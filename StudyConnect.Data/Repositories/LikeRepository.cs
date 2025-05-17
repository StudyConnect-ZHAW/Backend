using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Common;
using StudyConnect.Core.Interfaces.Repositories;
using StudyConnect.Core.Models;

namespace StudyConnect.Data.Repositories;

public class LikeRepository : BaseRepository, ILikeRepository
{
    public LikeRepository(StudyConnectDbContext context) : base(context)
    {

    }

    public async Task<bool> CommentLikeExistsAsync(Guid userId, Guid commentId) =>
        await _context.ForumLikes.AnyAsync(l => l.UserId == userId && l.ForumCommentId == commentId);

    public async Task<int> GetCommentLikeCountAsync(Guid commentId) =>
        await _context.ForumLikes.CountAsync(l => l.ForumCommentId == commentId);

    public async Task<int> GetPostLikeCountAsync(Guid postId) =>
        await _context.ForumLikes.CountAsync(l => l.ForumPostId == postId);

    public async Task LikeCommentAsync(Guid userId, Guid commentId)
    {
        var newLike = new Entities.ForumLike
        {
            UserId = userId,
            ForumCommentId = commentId
        };

        await _context.ForumLikes.AddAsync(newLike);
        await _context.SaveChangesAsync();
    }

    public async Task LikePostAsync(Guid userId, Guid postId)
    {
        var newLike = new Entities.ForumLike
        {
            UserId = userId,
            ForumPostId = postId
        };
        ;
        await _context.ForumLikes.AddAsync(newLike);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> PostLikeExistsAsync(Guid userId, Guid postId) =>
        await _context.ForumLikes.AnyAsync(l => l.UserId == userId && l.ForumPostId == postId);

    public async Task UnlikeCommentAsync(Guid userId, Guid commentId)
    {
        var toDelete = _context.ForumLikes.FirstOrDefaultAsync(l => l.UserId == userId && l.ForumCommentId == commentId);

        _context.Remove(toDelete!);
        await _context.SaveChangesAsync();
    }

    public async Task UnlikePostAsync(Guid userId, Guid postId)
    {
        var toDelete = _context.ForumLikes.FirstOrDefaultAsync(l => l.UserId == userId && l.ForumCommentId == postId);

        _context.Remove(toDelete!);
        await _context.SaveChangesAsync();
    }
}

