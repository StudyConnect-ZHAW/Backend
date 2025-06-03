using StudyConnect.Core.Common;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces;

public interface ILikeRepository
{
    /// <summary>
    /// Add a like to forum post.
    /// </summary>
    /// <param name="userId">The unique identifier of user, who has left a like.</param>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns>The <see cref="Guid"/> of the newly created like.</returns>
    Task<OperationResult<bool>> LikePostAsync(Guid userId, Guid postId);

    /// <summary>
    /// Add a like to forum comment.
    /// </summary>
    /// <param name="userId">The unique identifier of user, who has left a like.</param>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <returns>The <see cref="Guid"/> of the newly created like.</returns>
    Task<OperationResult<bool>> LikeCommentAsync(Guid userId, Guid commentId);

    /// <summary>
    /// Remove a like from forum post.
    /// </summary>
    /// <param name="userId">The unique identifier of user, who has left a like.</param>
    /// <param name="postId">The unique identifier of the post.</param>
    Task<OperationResult<bool>> UnlikePostAsync(Guid userId, Guid postId);

    /// <summary>
    /// Remove a like from forum comment.
    /// </summary>
    /// <param name="userId">The unique identifier of user, who has left a like.</param>
    /// <param name="commentId">The unique identifier of the comment.</param>
    Task<OperationResult<bool>> UnlikeCommentAsync(Guid userId, Guid commentId);

    /// <summary>
    /// Get the amount of likes for a given forum post.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns>The number of likes for the post.</returns>
    Task<int> GetPostLikeCountAsync(Guid postId);

    /// <summary>
    /// Get the likes for a given user.
    /// </summary>
    /// <param name="userId">the unique identifier of the current user.</param>
    /// <returns>The number of likes for the post.</returns>
    Task<OperationResult<IEnumerable<ForumLike>>> GetPostLikesForUser(Guid userId);

    /// <summary>
    /// Get the amount of likes for a given forum comment.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <returns>The number of likes for the comment.</returns>
    Task<int> GetCommentLikeCountAsync(Guid commentId);

    /// <summary>
    /// Get the likes for a given user.
    /// </summary>
    /// <param name="userId">the unique identifier of the current user.</param>
    /// <returns>The number of likes for the post.</returns>
    Task<OperationResult<IEnumerable<ForumLike>>> GetCommentLikesForUser(Guid userId);

    /// <summary>
    /// Checks whether a like for a post was already made.
    /// </summary>
    /// <param name="userId">The unique identifier of user, who has left a like.</param>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns><c>true</c> if the like exists; otherwise, <c>false</c>.</returns>
    Task<bool> PostLikeExistsAsync(Guid userId, Guid postId);

    /// <summary>
    /// Checks whether a like for a comment was already made.
    /// </summary>
    /// <param name="userId">The unique identifier of user, who has left a like.</param>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <returns><c>true</c> if the like exists; otherwise, <c>false</c>.</returns>
    Task<bool> CommentLikeExistsAsync(Guid userId, Guid commentId);
}
