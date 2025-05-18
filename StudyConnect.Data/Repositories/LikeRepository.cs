using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Interfaces.Repositories;
using StudyConnect.Core.Common;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.Data.Repositories;

public class LikeRepository : BaseRepository, ILikeRepository
{
    public LikeRepository(StudyConnectDbContext context) : base(context) { }

    public async Task<bool> CommentLikeExistsAsync(Guid userId, Guid commentId) =>
        await _context.ForumLikes.AnyAsync(l => l.UserId == userId && l.ForumCommentId == commentId);

    public async Task<bool> PostLikeExistsAsync(Guid userId, Guid postId) =>
        await _context.ForumLikes.AnyAsync(l => l.UserId == userId && l.ForumPostId == postId);

    public async Task<OperationResult<int>> GetCommentLikeCountAsync(Guid commentId)
    {
        if (commentId == Guid.Empty || !await _context.ForumComments.AnyAsync(cm => cm.ForumCommentId == commentId))
            return OperationResult<int>.Failure(CommentNotFound);

        var result = await _context.ForumLikes.CountAsync(l => l.ForumCommentId == commentId);
        return OperationResult<int>.Success(result);

    }

    public async Task<OperationResult<int>> GetPostLikeCountAsync(Guid postId)
    {
        if (postId == Guid.Empty || !await _context.ForumPosts.AnyAsync(p => p.ForumPostId == postId))
            return OperationResult<int>.Failure(PostNotFound);

        var result = await _context.ForumLikes.CountAsync(l => l.ForumPostId == postId);
        return OperationResult<int>.Success(result);
    }


    public async Task<OperationResult<bool>> LikeCommentAsync(Guid userId, Guid commentId)
    {
        if (userId == Guid.Empty || !await _context.Users.AnyAsync(u => u.UserGuid == userId))
            return OperationResult<bool>.Failure(UserNotFound);

        if (commentId == Guid.Empty || !await _context.ForumComments.AnyAsync(cm => cm.ForumCommentId == commentId))
            return OperationResult<bool>.Failure(CommentNotFound);

        if (await CommentLikeExistsAsync(userId, commentId))
            return OperationResult<bool>.Failure("Like already exists.");

        var newLike = new Entities.ForumLike
        {
            UserId = userId,
            ForumCommentId = commentId
        };

        await _context.ForumLikes.AddAsync(newLike);
        await _context.SaveChangesAsync();

        return OperationResult<bool>.Success(true);
    }

    public async Task<OperationResult<bool>> LikePostAsync(Guid userId, Guid postId)
    {
        if (userId == Guid.Empty || !await _context.Users.AnyAsync(u => u.UserGuid == userId))
            return OperationResult<bool>.Failure(UserNotFound);

        if (postId == Guid.Empty || !await _context.ForumPosts.AnyAsync(p => p.ForumPostId == postId))
            return OperationResult<bool>.Failure(PostNotFound);

        if (await CommentLikeExistsAsync(userId, postId))
            return OperationResult<bool>.Failure("Like already exists.");

        var newLike = new Entities.ForumLike
        {
            UserId = userId,
            ForumPostId = postId
        };

        await _context.ForumLikes.AddAsync(newLike);
        await _context.SaveChangesAsync();

        return OperationResult<bool>.Success(true);
    }

    public async Task<OperationResult<bool>> UnlikeCommentAsync(Guid userId, Guid commentId)
    {
        if (userId == Guid.Empty || !await _context.Users.AnyAsync(u => u.UserGuid == userId))
            return OperationResult<bool>.Failure(UserNotFound);

        if (commentId == Guid.Empty || !await _context.ForumComments.AnyAsync(cm => cm.ForumCommentId == commentId))
            return OperationResult<bool>.Failure(CommentNotFound);

        var toDelete = await _context.ForumLikes
            .FirstOrDefaultAsync(l => l.UserId == userId && l.ForumCommentId == commentId);

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

        if (postId == Guid.Empty || !await _context.ForumPosts.AnyAsync(p => p.ForumPostId == postId))
            return OperationResult<bool>.Failure(PostNotFound);

        var toDelete = await _context.ForumLikes
            .FirstOrDefaultAsync(l => l.UserId == userId && l.ForumPostId == postId);

        if (toDelete is not null)
        {
            _context.ForumLikes.Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        return OperationResult<bool>.Success(true);
    }
}
