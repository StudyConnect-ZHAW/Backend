using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using StudyConnect.Core.Common;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.Data.Repositories;

public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(StudyConnectDbContext context) : base(context)
    {

    }

    public async Task<OperationResult<ForumComment?>> AddAsync(ForumComment comment, Guid userId, Guid postId, Guid? parentId)
    {
        // Validate the user ID and retrieve the corresponding user entity
        var userResult = await GetUserAsync(userId);
        if (!userResult.IsSuccess) return OperationResult<ForumComment?>.Failure(userResult.ErrorMessage!);

        // Validate the post ID and retrieve the corresponding forum post entity
        var postResult = await GetPostAsync(postId);
        if (!postResult.IsSuccess) return OperationResult<ForumComment?>.Failure(postResult.ErrorMessage!);

        var user = userResult.Data!;
        var post = postResult.Data!;

        Entities.ForumComment? parent = null;

        // Check if a parent comment ID was provided
        if (parentId.HasValue)
        {
            // Attempt to retrieve the parent comment from the database
            parent = await _context.ForumComments
                .FirstOrDefaultAsync(c => c.ForumCommentId == parentId && c.ForumPost.ForumPostId == postId);

            // Test for data
            if (parent == null)
                return OperationResult<ForumComment?>.Failure(ParentCommentNotFound);
        }

        try
        {
            // Increment the reply count for the parent comment and post
            if (parent != null) parent.ReplyCount++;
            post.CommentCount++;

            // Create the comment entity and populate it with the relevant data
            var result = new Entities.ForumComment
            {
                Content = comment.Content,
                User = user,
                ForumPost = post,
                ParentComment = parent
            };

            await _context.AddAsync(result);
            await _context.SaveChangesAsync();

            return OperationResult<ForumComment?>.Success(MapCommentToModel(result));
        }
        catch (Exception ex)
        {

            return OperationResult<ForumComment?>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<IEnumerable<ForumComment>?>> GetAllofPostAsync(Guid postId)
    {
        if (IsInvalid(postId))
            return OperationResult<IEnumerable<ForumComment>?>.Failure(InvalidPostId);

        // Retrieve all comments for the specified post, including related entities
        var comments = await _context.ForumComments
            .AsNoTracking()
            .Include(c => c.User)
            .Include(c => c.ForumPost)
            .Include(c => c.ParentComment)
            .Where(c => c.ForumPost.ForumPostId == postId)
            .ToListAsync();

        // Test list for data
        if (comments.Count == 0)
            return OperationResult<IEnumerable<ForumComment>?>.Failure(CommentsNotFound);

        // Build the parent-child relationships among the comment entities
        BuildEntityCommentTree(comments);

        // Convert top-level comments to model format
        var result = comments
          .Where(c => c.ParentComment == null)
          .Select(MapCommentToModel);

        return OperationResult<IEnumerable<ForumComment>?>.Success(result);
    }

    public async Task<OperationResult<ForumComment?>> GetByIdAsync(Guid commentId)
    {
        if (IsInvalid(commentId))
            return OperationResult<ForumComment?>.Failure(InvalidCommentId);

        // Retrieve a comment including related entities
        var comment = await _context.ForumComments
            .AsNoTracking()
            .Include(c => c.User)
            .Include(c => c.ForumPost)
            .FirstOrDefaultAsync(c => c.ForumCommentId == commentId);

        // Test for data
        if (comment == null)
            return OperationResult<ForumComment?>.Failure(CommentNotFound);

        // Convert comment to model format
        var result = MapCommentToModel(comment);

        return OperationResult<ForumComment?>.Success(result);
    }

    public async Task<OperationResult<bool>> UpdateAsync(Guid commentId, Guid userId, ForumComment comment)
    {
        if (IsInvalid(commentId))
            return OperationResult<bool>.Failure(InvalidCommentId);

        if (IsInvalid(userId))
            return OperationResult<bool>.Failure(InvalidUserId);

        // Retrieve the comment and ensure the user is authorized to access it
        var result = await GetAuthorizedCommentAsync(commentId, userId);
        if (!result.IsSuccess)
            return OperationResult<bool>.Failure(result.ErrorMessage!);

        try
        {
            // Update the comment's content and metadata
            result.Data!.Content = comment.Content;
            result.Data!.UpdatedAt = DateTime.UtcNow;
            result.Data!.IsEdited = true;

            await _context.SaveChangesAsync();

            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid commentId, Guid userId)
    {
        if (IsInvalid(commentId))
            return OperationResult<bool>.Failure(InvalidCommentId);

        if (IsInvalid(userId))
            return OperationResult<bool>.Failure(InvalidUserId);

        // Retrieve the comment and ensure the user is authorized to access it
        var result = await GetAuthorizedCommentAsync(commentId, userId);
        if (!result.IsSuccess)
            return OperationResult<bool>.Failure(result.ErrorMessage!);

        try
        {
            result.Data!.IsDeleted = true;
            result.Data!.IsEdited = false;
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    /// <summary>
    /// A helper function to test whether a GUID is valid.
    /// </summary>
    /// <param name="id">The guid to test.</param>
    /// <returns>A boolean.</returns>
    private static bool IsInvalid(Guid id) => id == Guid.Empty;

    /// <summary>
    /// A helper function to fetch user data from the database.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    private async Task<OperationResult<Entities.User?>> GetUserAsync(Guid userId)
    {
        if (IsInvalid(userId))
            return OperationResult<Entities.User?>.Failure(InvalidUserId);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserGuid == userId);
        return user is null
            ? OperationResult<Entities.User?>.Failure(UserNotFound)
            : OperationResult<Entities.User?>.Success(user);
    }

    /// <summary>
    /// A helper function to fetch forum post data from the database.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    private async Task<OperationResult<Entities.ForumPost?>> GetPostAsync(Guid postId)
    {
        if (IsInvalid(postId))
            return OperationResult<Entities.ForumPost?>.Failure(InvalidPostId);

        var post = await _context.ForumPosts
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .FirstOrDefaultAsync(p => p.ForumPostId == postId);

        return post is null
            ? OperationResult<Entities.ForumPost?>.Failure(PostNotFound)
            : OperationResult<Entities.ForumPost?>.Success(post);
    }

    /// <summary>
    /// Retrieves a forum comment by its ID and verifies that the specified user is the author.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <param name="userId">The unique identifier of the user requesting access.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    private async Task<OperationResult<Entities.ForumComment?>> GetAuthorizedCommentAsync(Guid commentId, Guid userId)
    {
        var comment = await _context.ForumComments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.ForumCommentId == commentId);

        if (comment == null)
            return OperationResult<Entities.ForumComment?>.Failure(CommentNotFound);

        if (comment.User.UserGuid != userId)
            return OperationResult<Entities.ForumComment?>.Failure(NotAuthorized);

        return OperationResult<Entities.ForumComment?>.Success(comment);
    }

    /// <summary>
    /// A helper function to establish parent-child relationships in a list of comment entities.
    /// </summary>
    /// <param name="comments">A list of comments, each of which may have a parent comment ID.</param>
    private void BuildEntityCommentTree(List<Entities.ForumComment> comments)
    {
        var commentDict = comments.ToDictionary(c => c.ForumCommentId);

        foreach (var comment in comments)
        {
            if (comment.ParentComment != null &&
                commentDict.TryGetValue(comment.ParentComment.ForumCommentId, out var parent))
            {
                parent.Replies ??= new List<Entities.ForumComment>();
                parent.Replies.Add(comment);
            }
        }
    }

    /// <summary>
    /// A helper function to map a forum comment entity to its model representation.
    /// </summary>
    /// <param name="comment">A comment entity to transform.</param>
    /// <returns>A forum comment model object.</returns>
    private ForumComment MapCommentToModel(Entities.ForumComment comment)
    {
        return new ForumComment
        {
            ForumCommentId = comment.ForumCommentId,
            Content = comment.IsDeleted ? "" : comment.Content,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            ReplyCount = comment.ReplyCount,
            IsEdited = comment.IsEdited,
            IsDeleted = comment.IsDeleted,
            PostId = comment.ForumPost.ForumPostId,
            ParentCommentId = comment.ParentComment != null
                ? comment.ParentComment.ForumCommentId
                : null,
            User = comment.IsDeleted
                ? null
                : ModelMapper.MapUserToModel(comment.User),
            Replies = comment.Replies != null
                ? comment.Replies.Select(MapCommentToModel).ToList()
                : null
        };
    }
}
