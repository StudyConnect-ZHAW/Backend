using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Interfaces.Repositories;
using StudyConnect.Core.Models;
using StudyConnect.Data.Utilities;

namespace StudyConnect.Data.Repositories;

public class LikeRepository : BaseRepository, ILikeRepository
{
    public LikeRepository(StudyConnectDbContext context) : base(context) { }

    public async Task<bool> CommentLikeExistsAsync(Guid userId, Guid commentId) =>
        await _context.ForumLikes.AnyAsync(l => l.UserId == userId && l.ForumCommentId == commentId);

    public async Task<bool> PostLikeExistsAsync(Guid userId, Guid postId) =>
        await _context.ForumLikes.AnyAsync(l => l.UserId == userId && l.ForumPostId == postId);

    public async Task<int> GetCommentLikeCountAsync(Guid commentId) =>
        await _context.ForumLikes.CountAsync(l => l.ForumCommentId == commentId);

    public async Task<int> GetPostLikeCountAsync(Guid postId) =>
        await _context.ForumLikes.CountAsync(l => l.ForumPostId == postId);

    public async Task<ForumLike?> GetLikeById(Guid likeId)
    {
        var like = await _context.ForumLikes
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.LikeId == likeId);

        return like?.ToLikeModel();
    }

    public async Task<Guid> LikeCommentAsync(Guid userId, Guid commentId)
    {
        var newLike = new Entities.ForumLike
        {
            UserId = userId,
            ForumCommentId = commentId
        };

        await _context.ForumLikes.AddAsync(newLike);
        await _context.SaveChangesAsync();

        return newLike.LikeId;
    }

    public async Task<Guid> LikePostAsync(Guid userId, Guid postId)
    {
        var newLike = new Entities.ForumLike
        {
            UserId = userId,
            ForumPostId = postId
        };

        await _context.ForumLikes.AddAsync(newLike);
        await _context.SaveChangesAsync();

        return newLike.LikeId;
    }

    public async Task UnlikeCommentAsync(Guid userId, Guid commentId)
    {
        var toDelete = await _context.ForumLikes
            .FirstOrDefaultAsync(l => l.UserId == userId && l.ForumCommentId == commentId);

        if (toDelete is not null)
        {
            _context.ForumLikes.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UnlikePostAsync(Guid userId, Guid postId)
    {
        var toDelete = await _context.ForumLikes
            .FirstOrDefaultAsync(l => l.UserId == userId && l.ForumPostId == postId);

        if (toDelete is not null)
        {
            _context.ForumLikes.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
    }
}

