using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces.Services;

/// <summary>
/// Defines the contract for like-related operations in the forum system.
/// </summary </summary>>
public interface ILikeService
{
    /// <summary>
    /// Leave a like to comment or post.
    /// </summary>
    /// <param name="userId">The unique identifier of user, who has left a like.</param>
    /// <param name="postId">The unique identifier of the post (optional).</param>
    /// <param name="commentId">The unique identifier of the comment (optional).</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<ForumLike>> LeaveLikeAsync(Guid userId, Guid? postId, Guid? commentId);

    /// <summary>
    /// Remove a like from comment or post.
    /// </summary>
    /// <param name="userId">The unique identifier of user, who has left a like.</param>
    /// <param name="postId">The unique identifier of the post (optional).</param>
    /// <param name="commentId">The unique identifier of the comment (optional).</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<bool>> RemoveLikeAsync(Guid userId, Guid? postId, Guid? commentId);

    /// <summary>
    /// Get like count for comment or post.
    /// </summary>
    /// <param name="postId">The unique identifier of the post (optional).</param>
    /// <param name="commentId">The unique identifier of the comment (optional).</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<int>> GetLikeCountAsync(Guid? postId, Guid? commentId);
}
