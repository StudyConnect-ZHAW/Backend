using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces;

public interface IGroupCommentRepository
{
    /// <summary>
    /// Adds an comment to a post or parent comment.
    /// </summary>
    /// <param name="comment">The model containing information about the comment.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <param name="groupId">The unique identifier of group the post belogns to.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<GroupComment>> AddAsync(Guid userId, Guid groupId, Guid postId, ForumComment comment);

    /// <summary>
    /// Get a comment by its GUID.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<GroupComment?>> GetByIdAsync(Guid commentId);

    /// <summary>
    /// Retrieves all comments for a specific post.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<IEnumerable<GroupComment>>> GetAllofPostAsync(Guid postId);

    /// <summary>
    /// Updates a comment by its GUID.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <param name="userId">The unique identifier of the current user.</param>
    /// <param name="comment">A comment model containing the updated content.</param>
    /// <param name="groupId">The unique identifier of group the post belogns to.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns> 
    Task<OperationResult<GroupComment>> UpdateAsync(Guid commentId, Guid userId, Guid groupId, GroupComment comment);

    /// <summary>
    /// Delete a comment by its GUID.
    /// </summary> 
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <param name="userId">The unique identifier of the current user.</param>
    /// <param name="groupId">The unique identifier of group the post belogns to.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<bool>> DeleteAsync(Guid commentId, Guid userId, Guid groupId);
}
