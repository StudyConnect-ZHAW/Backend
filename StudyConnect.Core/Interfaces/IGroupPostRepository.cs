using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces;

public interface IGroupPostRepository
{
    /// <summary>
    /// Adds a new forum post to the database.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who created the post.</param>
    /// <param name="groupId">The unique identifier of group the post belongs to.</param>
    /// <param name="post">The model containing information about the forum post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<GroupPost>> AddAsync(Guid userId, Guid groupId, GroupPost? post);

    /// <summary>
    /// Retrieves a post by its unique identifier.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param> 
    /// <returns>An <see cref="OperationResult{T}"/> containing the post if found, or an error message if not.</returns>
    Task<OperationResult<GroupPost?>> GetByIdAsync(Guid postId);

    /// <summary>
    /// Get all Post of the forum.
    /// </summary>
    /// <param name="groupId">the unique identifier of the group the post belongs to.</param>
    /// <returns>An <see cref="OperationResult{T}"/> containing a list of post if found, or an error message if not.</returns>
    Task<OperationResult<IEnumerable<GroupPost>>> GetAllAsync(Guid groupId);

    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who created the post.</param>
    /// <param name="groupId">the unique identifier of the group the post belongs to.</param>
    /// <param name="postId">The unique identifier of the post, that should be updated.</param>
    /// <param name="post">The model containing the information about the forum post for the update.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<GroupPost>> UpdateAsync(Guid userId, Guid groupId, Guid postId, GroupPost post);

    /// <summary>
    /// Deletes an existing post.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who created the post.</param>
    /// <param name="groupId">the unique identifier of the group the post belongs to.</param>
    /// <param name="postId">The unique identifier of the post, that shoudl be deleted.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<bool>> DeleteAsync(Guid userId, Guid groupId, Guid postId);
}
