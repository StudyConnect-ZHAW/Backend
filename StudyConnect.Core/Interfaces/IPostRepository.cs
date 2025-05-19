using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces;

public interface IPostRepository
{
    /// <summary>
    /// Adds a new forum post to the database.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who created the post.</param>
    /// <param name="categoryId">The unique identifier of category the post belogns to.</param>
    /// <param name="post">The model containing information about the forum post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<ForumPost>> AddAsync(Guid userId, Guid categoryId, ForumPost? post);

    /// <summary>
    /// Search posts based on input.
    /// </summary>
    /// <param name="authorId">The unique identifier of the post creator.</param>
    /// <param name="categoryName">The unique name of category assigned to the post.</param>
    /// <param name="title">The title of the post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> containing a list of post if found, or an error message if not.</returns>
    Task<OperationResult<IEnumerable<ForumPost>>> SearchAsync(Guid? userId, string? categoryName, string? title);

    /// <summary>
    /// Retrieves a post by its unique identifier.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param> 
    /// <returns>An <see cref="OperationResult{T}"/> containing the post if found, or an error message if not.</returns>
    Task<OperationResult<ForumPost?>> GetByIdAsync(Guid postId);

    /// <summary>
    /// Get all Post of the forum.
    /// </summary>
    /// <returns>An <see cref="OperationResult{T}"/> containing a list of post if found, or an error message if not.</returns>
    Task<OperationResult<IEnumerable<ForumPost>>> GetAllAsync();

    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who created the post.</param>
    /// <param name="postId">The unique identifier of the post, that shoudl be updated.</param>
    /// <param name="post">The model containing the information about the forum post for the update.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<ForumPost>> UpdateAsync(Guid userId, Guid postId, ForumPost post);

    /// <summary>
    /// Deletes an exsiting post.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who created the post.</param>
    /// <param name="postId">The unique identifier of the post, that shoudl be deleted.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<bool>> DeleteAsync(Guid userId, Guid postId);
}
