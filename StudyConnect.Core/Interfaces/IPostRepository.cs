using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces;

public interface IPostRepository
{
    /// <summary>
    /// Add a Post to the database.
    /// </summary>
    /// <param name="post">The forumpost to be added.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<Guid?>> AddAsync(Guid userId, Guid forumId, ForumPost? post);

    /// <summary>
    /// Search posts based on input.
    /// </summary>
    /// <param name="authorId">The unique identifier of the post creater.</param>
    /// <param name="categoryName">The unique name of category assigned to the post.</param>
    /// <param name="title">The title of the post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> containing a list of post if found, or an error message if not.</returns>
    Task<OperationResult<IEnumerable<ForumPost>>> SearchAsync(Guid? userId, string? categoryName, string? title);

    /// <summary>
    /// Get a Post by its GUID.
    /// </summary>
    /// <param name="id">the unique identifier of the post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> containing the post if found, or an error message if not.</returns>
    Task<OperationResult<ForumPost?>> GetByIdAsync(Guid id);

    /// <summary>
    /// Get all Post of the forum.
    /// </summary>
    /// <returns>An <see cref="OperationResult{T}"/> containing a list of post if found, or an error message if not.</returns>
    Task<OperationResult<IEnumerable<ForumPost>>> GetAllAsync();

    /// <summary>
    /// Update a Post by its GUID.
    /// </summary>
    /// <param name="id">The unique identifier of the post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<bool>> UpdateAsync(Guid id, ForumPost post);

    /// <summary>
    /// Delete an exitsting post by its GUID.
    /// </summary>
    /// <param name="id">The unique identifier of the post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<bool>> DeleteAsync(Guid id);
}
