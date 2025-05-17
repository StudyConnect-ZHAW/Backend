using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Interfaces.Repositories;
using StudyConnect.Core.Models;
using StudyConnect.Data.Utilities;

namespace StudyConnect.Data.Repositories;

public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(StudyConnectDbContext context) : base(context) { }

    public async Task<Guid> AddAsync(ForumComment comment, Guid userId, Guid postId, Guid? parentId)
    {
        var entity = new Entities.ForumComment
        {
            Content = comment.Content,
            UserId = userId,
            ForumPostId = postId,
            ParentCommentId = parentId
        };

        await _context.ForumComments.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity.ForumCommentId;
    }

    public async Task<IEnumerable<ForumComment>> GetAllofPostAsync(Guid postId)
    {
        var allComments = await _context.ForumComments
            .AsNoTracking()
            .Where(c => c.ForumPostId == postId)
            .ToListAsync();

        return allComments
            .Where(c => c.ParentCommentId == null)
            .Select(c => c.ToCommentModel());
    }

    public async Task<ForumComment?> GetByIdAsync(Guid commentId)
    {
        var comment = await _context.ForumComments
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ForumCommentId == commentId);

        return comment?.ToCommentModel();
    }

    public async Task UpdateAsync(Guid commentId, ForumComment comment)
    {
        var existing = await _context.ForumComments
            .FirstOrDefaultAsync(c => c.ForumCommentId == commentId);

        if (existing == null) return;

        existing.Content = comment.Content;
        existing.UpdatedAt = DateTime.UtcNow;
        existing.IsEdited = true;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid commentId)
    {
        var comment = await _context.ForumComments
            .FirstOrDefaultAsync(c => c.ForumCommentId == commentId);

        if (comment == null) return;

        comment.IsDeleted = true;
        comment.IsEdited = false;

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid commentId) =>
        await _context.ForumComments.AnyAsync(c => c.ForumCommentId == commentId);

    public async Task<bool> ContainsPostAsync(Guid postId, Guid commentId) =>
        await _context.ForumComments.AnyAsync(c =>
            c.ForumCommentId == commentId && c.ForumPostId == postId);

    public async Task<bool> ContainsUserAsync(Guid userId, Guid commentId) =>
        await _context.ForumComments.AnyAsync(c =>
            c.ForumCommentId == commentId && c.UserId == userId);

    public async Task IncrementReplyCountAsync(Guid commentId)
    {
        var comment = await _context.ForumComments.FindAsync(commentId);
        if (comment == null) return;

        comment.ReplyCount++;
        _context.Entry(comment).Property(c => c.ReplyCount).IsModified = true;

        await _context.SaveChangesAsync();
    }
}

