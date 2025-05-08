using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using StudyConnect.Core.Common;

namespace StudyConnect.Data.Repositories;

public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(StudyConnectDbContext context) : base(context)
    {

    }

    public async Task<OperationResult<ForumComment?>> AddAsync(ForumComment comment, Guid userId, Guid postId, Guid? parentId)
    {
        Entities.ForumComment? parent = null;

        if (userId == Guid.Empty)
            return OperationResult<ForumComment?>.Failure("Invalid user Id.");

        if (postId == Guid.Empty)
            return OperationResult<ForumComment?>.Failure("Invalid post Id.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserGuid == userId);
        var post = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == userId);

        if (post == null || user == null)
            return OperationResult<ForumComment?>.Failure("Not found.");

        if (parentId.HasValue)
        {
            parent = await _context.ForumComments.FirstOrDefaultAsync(c => c.ForumCommentId == parentId);
            if (parent == null)
                return OperationResult<ForumComment?>.Failure("Not found.");
        }

        try
        {
            var result = new Entities.ForumComment
            {
                Content = comment.Content,
                User = user,
                ForumPost = post,
                ParentComment = parent
            };

            await _context.AddAsync(result);
            await _context.SaveChangesAsync();

            if (parent != null)
                parent.ReplyCount++;

            post.CommentCount++;

            return OperationResult<ForumComment?>.Success(PackageComment(result));
        }
        catch (Exception ex)
        {
            return OperationResult<ForumComment?>.Failure($"Failed to add the comment: {ex.Message}");
        }
    }

    public async Task<OperationResult<IEnumerable<ForumComment>?>> GetAllofPostAsync(Guid postId)
    {
        if (postId == Guid.Empty)
            return OperationResult<IEnumerable<ForumComment>?>.Failure("Invalid post id.");

        var comments = await _context.ForumComments
            .AsNoTracking()
            .Include(c => c.User)
            .Include(c => c.ForumPost)
            .Include(c => c.Replies)
            .ToListAsync();

        if (comments.Count == 0)
            return OperationResult<IEnumerable<ForumComment>?>.Success(null);

        var result = comments
           .Where(c => c.ParentComment == null)
           .Select(c => PackageCommentTree(c));

        return OperationResult<IEnumerable<ForumComment>?>.Success(result);
    }

    public async Task<OperationResult<ForumComment?>> GetByIdAsync(Guid commentId)
    {
        if (commentId == Guid.Empty)
            return OperationResult<ForumComment?>.Failure("Invalid comment Id");


        var comment = await _context.ForumComments.FirstOrDefaultAsync(c => c.ForumCommentId == commentId);
        if (comment == null)
            return OperationResult<ForumComment?>.Success(null);

        var result = PackageComment(comment);

        return OperationResult<ForumComment?>.Success(result);
    }

    public async Task<OperationResult<bool>> UpdateAsync(Guid commentId, ForumComment comment)
    {
        if (commentId == Guid.Empty)
            return OperationResult<bool>.Failure("Invalid comment Id.");

        if (comment == null)
            return OperationResult<bool>.Failure("Post cannot be null.");

        var commentToUpdate = await _context.ForumComments.FirstOrDefaultAsync(c => c.ForumCommentId == commentId);
        if (commentToUpdate == null)
            return OperationResult<bool>.Success(false);

        try
        {
            commentToUpdate.Content = comment.Content;
            commentToUpdate.UpdatedAt = DateTime.UtcNow;
            commentToUpdate.IsEdited = true;

            await _context.SaveChangesAsync();

            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"An error occurred while updating: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid commentId)
    {
        if (commentId == Guid.Empty)
            return OperationResult<bool>.Failure("Invalid comment Id.");

        var commentToDelete = await _context.ForumComments.FirstOrDefaultAsync(c => c.ForumCommentId == commentId);
        if (commentToDelete == null)
            return OperationResult<bool>.Success(false);

        try
        {
            commentToDelete.IsDeleted = true;
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"An error occurred while deleting: {ex:Message}");
        }
    }
}

