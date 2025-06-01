using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Common;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.Data.Repositories;

public class LikeRepository : BaseRepository, ILikeRepository
{
    public LikeRepository(StudyConnectDbContext context)
        : base(context) { }

    public async Task<bool> CommentLikeExistsAsync(Guid userId, Guid commentId) =>
        await _context.ForumLikes.AnyAsync(l =>
            l.UserId == userId && l.ForumCommentId == commentId
        );

    public async Task<bool> PostLikeExistsAsync(Guid userId, Guid postId) =>
        await _context.ForumLikes.AnyAsync(l => l.UserId == userId && l.ForumPostId == postId);

    public async Task<int> GetCommentLikeCountAsync(Guid commentId) =>
        await _context.ForumLikes.CountAsync(l => l.ForumCommentId == commentId);

    public async Task<int> GetPostLikeCountAsync(Guid postId) =>
        await _context.ForumLikes.CountAsync(l => l.ForumPostId == postId);

    public async Task<OperationResult<bool>> LikeCommentAsync(Guid userId, Guid commentId)
    {
        if (userId == Guid.Empty || !await _context.Users.AnyAsync(u => u.UserGuid == userId))
            return OperationResult<bool>.Failure(UserNotFound);

        if (
            commentId == Guid.Empty
            || !await _context.ForumComments.AnyAsync(cm => cm.ForumCommentId == commentId)
        )
            return OperationResult<bool>.Failure(CommentNotFound);

        if (await CommentLikeExistsAsync(userId, commentId))
            return OperationResult<bool>.Failure("Like already exists.");

        var newLike = new Entities.ForumLike { UserId = userId, ForumCommentId = commentId };

        await _context.ForumLikes.AddAsync(newLike);
        await _context.SaveChangesAsync();

        return OperationResult<bool>.Success(true);
    }

    public async Task<OperationResult<bool>> LikePostAsync(Guid userId, Guid postId)
    {
        if (userId == Guid.Empty || !await _context.Users.AnyAsync(u => u.UserGuid == userId))
            return OperationResult<bool>.Failure(UserNotFound);

        if (
            postId == Guid.Empty
            || !await _context.ForumPosts.AnyAsync(p => p.ForumPostId == postId)
        )
            return OperationResult<bool>.Failure(PostNotFound);

        if (await CommentLikeExistsAsync(userId, postId))
            return OperationResult<bool>.Failure("Like already exists.");

        var newLike = new Entities.ForumLike { UserId = userId, ForumPostId = postId };

        await _context.ForumLikes.AddAsync(newLike);
        await _context.SaveChangesAsync();

        return OperationResult<bool>.Success(true);
    }

    public async Task<OperationResult<bool>> UnlikeCommentAsync(Guid userId, Guid commentId)
    {
        if (userId == Guid.Empty || !await _context.Users.AnyAsync(u => u.UserGuid == userId))
            return OperationResult<bool>.Failure(UserNotFound);

        if (
            commentId == Guid.Empty
            || !await _context.ForumComments.AnyAsync(cm => cm.ForumCommentId == commentId)
        )
            return OperationResult<bool>.Failure(CommentNotFound);

        var toDelete = await _context.ForumLikes.FirstOrDefaultAsync(l =>
            l.UserId == userId && l.ForumCommentId == commentId
        );

        if (toDelete is not null)
        {
            _context.ForumLikes.Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        return OperationResult<bool>.Success(true);
    }

    public async Task<OperationResult<bool>> UnlikePostAsync(Guid userId, Guid postId)
    {
        if (userId == Guid.Empty || !await _context.Users.AnyAsync(u => u.UserGuid == userId))
            return OperationResult<bool>.Failure(UserNotFound);

        if (
            postId == Guid.Empty
            || !await _context.ForumPosts.AnyAsync(p => p.ForumPostId == postId)
        )
            return OperationResult<bool>.Failure(PostNotFound);

        var toDelete = await _context.ForumLikes.FirstOrDefaultAsync(l =>
            l.UserId == userId && l.ForumPostId == postId
        );

        if (toDelete is not null)
        {
            _context.ForumLikes.Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        return OperationResult<bool>.Success(true);
    }

    public async Task<OperationResult<IEnumerable<ForumLike>>> GetPostLikesForUser(
        Guid userId,
        Guid postId
    )
    {
        if (userId == Guid.Empty || !await _context.Users.AnyAsync(u => u.UserGuid == userId))
            return OperationResult<IEnumerable<ForumLike>>.Failure(UserNotFound);

        if (
            postId == Guid.Empty
            || !await _context.ForumPosts.AnyAsync(p => p.ForumPostId == postId)
        )
            return OperationResult<IEnumerable<ForumLike>>.Failure(PostNotFound);

        var likes = await _context
            .ForumLikes.Where(l => l.UserId == userId && l.ForumPostId == postId)
            .ToListAsync();

        return OperationResult<IEnumerable<ForumLike>>.Success(
            likes.Select(l => l.ToForumLikeModel())
        );
    }

    public async Task<OperationResult<IEnumerable<ForumLike>>> GetCommentLikesForUser(
        Guid userId,
        Guid commentId
    )
    {
        if (userId == Guid.Empty || !await _context.Users.AnyAsync(u => u.UserGuid == userId))
            return OperationResult<IEnumerable<ForumLike>>.Failure(UserNotFound);

        if (
            commentId == Guid.Empty
            || !await _context.ForumComments.AnyAsync(p => p.ForumPostId == commentId)
        )
            return OperationResult<IEnumerable<ForumLike>>.Failure(PostNotFound);

        var likes = await _context
            .ForumLikes.Where(l => l.UserId == userId && l.ForumPostId == commentId)
            .ToListAsync();

        return OperationResult<IEnumerable<ForumLike>>.Success(
            likes.Select(l => l.ToForumLikeModel())
        );
    }
}
