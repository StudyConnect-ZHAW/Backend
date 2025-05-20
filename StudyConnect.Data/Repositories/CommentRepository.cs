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

    public async Task<OperationResult<ForumComment>> AddAsync(ForumComment comment, Guid userId, Guid postId, Guid? parentId)
    {
        // Validate the user ID and retrieve the corresponding user entity
        if (!await IsValidUser(userId))
            return OperationResult<ForumComment>.Failure(UserNotFound);

        // Validate the post ID and retrieve the corresponding forum post entity
        if (!await IsValidPost(postId))
            return OperationResult<ForumComment>.Failure(PostNotFound);

        if (parentId.HasValue && !await IsValidParent(parentId, postId))
            return OperationResult<ForumComment>.Failure(ParentCommentNotFound);

        try
        {
            // Create the comment entity and populate it with the relevant data
            var result = new Entities.ForumComment
            {
                Content = comment.Content,
                UserId = userId,
                ForumPostId = postId,
                ParentCommentId = parentId
            };

            await _context.AddAsync(result);
            await _context.SaveChangesAsync();

            await _context.ForumComments.Entry(result)
                .Reference(cm => cm.User)
                .LoadAsync();

            return OperationResult<ForumComment>.Success(MapCommentToModel(result));
        }
        catch (Exception ex)
        {
            return OperationResult<ForumComment>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<IEnumerable<ForumComment>>> GetAllofPostAsync(Guid postId)
    {
        if (!await IsValidPost(postId))
            return OperationResult<IEnumerable<ForumComment>>.Failure(PostNotFound);

        // Retrieve all comments for the specified post, including related entities
        var comments = await _context.ForumComments
            .AsNoTracking()
            .Include(cm => cm.User)
            .Include(cm => cm.ForumLikes)
            .Where(cm => cm.ForumPost.ForumPostId == postId)
            .ToListAsync();

        // Test list for data
        if (!comments.Any())
            return OperationResult<IEnumerable<ForumComment>>.Success(new List<ForumComment>());

        // Build the parent-child relationships among the comment entities
        BuildEntityCommentTree(comments);

        // Convert top-level comments to model format
        var result = comments
          .Where(cm => cm.ParentCommentId == null)
          .Select(MapCommentToModel);

        return OperationResult<IEnumerable<ForumComment>>.Success(result);
    }

    public async Task<OperationResult<ForumComment?>> GetByIdAsync(Guid commentId)
    {
        if (commentId == Guid.Empty)
            return OperationResult<ForumComment?>.Failure(InvalidCommentId);

        // Retrieve a comment including related entities
        var result = await _context.ForumComments
            .AsNoTracking()
            .Include(cm => cm.User)
            .Include(cm => cm.ForumLikes)
            .FirstOrDefaultAsync(c => c.ForumCommentId == commentId);

        // Test for data
        if (result == null)
            return OperationResult<ForumComment?>.Failure(CommentNotFound);

        return OperationResult<ForumComment?>.Success(MapCommentToModel(result));
    }

    public async Task<OperationResult<ForumComment>> UpdateAsync(Guid userId, Guid commentId, ForumComment comment)
    {
        if (userId == Guid.Empty)
            return OperationResult<ForumComment>.Failure(InvalidUserId);

        if (commentId == Guid.Empty)
            return OperationResult<ForumComment>.Failure(InvalidCommentId);

        // Retrieve the comment and ensure the user is authorized to access it
        var (result, error) = await GetAuthorizedCommentAsync(userId, commentId);
        if (result == null)
            return OperationResult<ForumComment>.Failure(error ?? UnknownError);

        try
        {
            // Update the comment's content and metadata
            result.Content = comment.Content;
            result.UpdatedAt = DateTime.UtcNow;
            result.IsEdited = true;

            await _context.SaveChangesAsync();

            return OperationResult<ForumComment>.Success(MapCommentToModel(result));
        }
        catch (Exception ex)
        {
            return OperationResult<ForumComment>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid userId, Guid commentId)
    {
        if (userId == Guid.Empty)
            return OperationResult<bool>.Failure(InvalidUserId);

        if (commentId == Guid.Empty)
            return OperationResult<bool>.Failure(InvalidCommentId);

        // Retrieve the comment and ensure the user is authorized to access it
        var (result, error) = await GetAuthorizedCommentAsync(userId, commentId);
        if (result == null)
            return OperationResult<bool>.Failure(error ?? UnknownError);

        try
        {
            result.IsDeleted = true;
            result.IsEdited = false;
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    /// <summary>
    /// Validates if a user exists in the database.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns><c>true</c> if the user exists; otherwise, <c>false</c>.</returns>
    private async Task<bool> IsValidUser(Guid userId) =>
           userId != Guid.Empty && await _context.Users.AnyAsync(u => u.UserGuid == userId);

    /// <summary>
    /// Validates if a post exists in the database.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns><c>true</c> if the post exists; otherwise, <c>false</c>.</returns>
    private async Task<bool> IsValidPost(Guid postId) =>
        postId != Guid.Empty && await _context.ForumPosts.AnyAsync(p => p.ForumPostId == postId);

    /// <summary>
    /// Validates if a parent comment exists in the database.
    /// </summary>
    /// <param name="parentid">The unique identifier of the parent comment.</param>
    /// <param name="postId">The unique identifier of the comment.</param>
    /// <returns><c>true</c> if the user exists; otherwise, <c>false</c>.</returns>
    private async Task<bool> IsValidParent(Guid? parentId, Guid postId) =>
        postId != Guid.Empty && await _context.ForumComments.AnyAsync(p => p.ForumCommentId == parentId && p.ForumPostId == postId);

    /// <summary>
    /// Tests a comment for existence and authorization.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="postId">The unique identifier of the comment.</param>
    /// <returns>A post entity on succes or an errormessage on failure.</returns>
    private async Task<(Entities.ForumComment? Comment, string? ErrorMessage)> GetAuthorizedCommentAsync(Guid userId, Guid commentId)
    {
        var comment = await _context.ForumComments
            .Include(cm => cm.User)
            .Include(cm => cm.ForumLikes)
            .FirstOrDefaultAsync(c => c.ForumCommentId == commentId);

        if (comment == null || comment.IsDeleted)
            return (null, CommentNotFound);

        if (comment.UserId != userId)
            return (null, NotAuthorized);

        return (comment, null);
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
            if (comment.ParentCommentId != null &&
                commentDict.TryGetValue(comment.ParentCommentId.Value, out var parent))
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
    ///
    private ForumComment MapCommentToModel(Entities.ForumComment comment)
    {
        return new ForumComment
        {
            ForumCommentId = comment.ForumCommentId,
            Content = comment.IsDeleted ? string.Empty : comment.Content,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            ReplyCount = comment.Replies.Count,
            IsEdited = comment.IsEdited,
            IsDeleted = comment.IsDeleted,
            PostId = comment.ForumPostId,
            ParentCommentId = comment.ParentCommentId,
            User = comment.IsDeleted ? null : comment.User.ToUserModel(),
            Replies = comment.Replies?.Select(MapCommentToModel).ToList(),
            LikeCount = comment.ForumLikes.Count
        };
    }
}
