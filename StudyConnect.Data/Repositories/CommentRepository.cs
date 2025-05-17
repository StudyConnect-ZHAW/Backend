using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Interfaces.Repositories;
using StudyConnect.Core.Models;
using StudyConnect.Data.Utilities;

namespace StudyConnect.Data.Repositories;

public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(StudyConnectDbContext context) : base(context)
    {

    }

    public async Task<Guid> AddAsync(ForumComment comment, Guid userId, Guid postId, Guid? parentId)
    {
        var result = new Entities.ForumComment
        {
            Content = comment.Content,
            UserId = userId,
            ForumPostId = postId,
            ParentCommentId = parentId
        };

        await _context.AddAsync(result);
        await _context.SaveChangesAsync();

        return result.ForumCommentId;
    }

    public async Task<IEnumerable<ForumComment>?> GetAllofPostAsync(Guid postId)
    {
        // Retrieve all comments for the specified post, including related entities
        var comments = await _context.ForumComments
            .AsNoTracking()
            .Where(c => c.ForumPost.ForumPostId == postId)
            .ToListAsync();

        // Convert top-level comments to model format
        var result = comments
          .Where(c => c.ParentComment == null)
          .Select(c => c.ToCommentModel());

        return result;
    }

    public async Task<ForumComment> GetByIdAsync(Guid commentId)
    {
        var comment = await _context.ForumComments
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ForumCommentId == commentId);

        return comment.ToCommentModel();
    }

    public async Task UpdateAsync(Guid commentId, ForumComment comment)
    {
        var result = await _context.ForumComments.FirstOrDefaultAsync(c => c.ForumCommentId == commentId);
        // Update the comment's content and metadata
        result!.Content = comment.Content;
        result!.UpdatedAt = DateTime.UtcNow;
        result!.IsEdited = true;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid commentId)
    {
        var result = await _context.ForumComments.FirstOrDefaultAsync(c => c.ForumCommentId == commentId);
        // Retrieve the comment and ensure the user is authorized to access it
        result!.IsDeleted = true;
        result!.IsEdited = false;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CommentExistsAsync(Guid commentId) =>
        await _context.ForumComments.AnyAsync(c => c.ForumCommentId == commentId);

    public async Task<bool> isChildOfPostAsync(Guid postId, Guid commentId) =>
        await _context.ForumComments.AnyAsync(c => c.ForumCommentId == commentId && c.ForumPostId == postId);

    public async Task<bool> isAuthorizedAsync(Guid userId, Guid commentId) =>
        await _context.ForumComments.AnyAsync(c => c.ForumCommentId == commentId && c.UserId == userId);

    public async Task IncrementReplyCountAsync(Guid commentId)
    {
        var comment = await _context.ForumComments.FindAsync(commentId);

        comment!.ReplyCount++;
        _context.Entry(comment).Property(c => c.ReplyCount).IsModified = true;
        await _context.SaveChangesAsync();
    }
}
