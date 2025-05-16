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
    Task<Guid> AddAsync(ForumPost post, Guid userId, Guid categoryId);

    /// <summary>
    /// Search posts based on input.
    /// </summary>
    /// <param name="authorId">The unique identifier of the post creator.</param>
    /// <param name="categoryName">The unique name of category assigned to the post.</param>
    /// <param name="title">The title of the post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> containing a list of post if found, or an error message if not.</returns>
    Task<IEnumerable<ForumPost?>> SearchAsync(
          Guid? userId,
          string? categoryName,
          string? title,
          DateTime? fromDate,
          DateTime? toDate
    );

    /// <summary>
    /// Get a Post by its GUID.
    /// </summary>
    /// <param name="id">the unique identifier of the post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> containing the post if found, or an error message if not.</returns>
    Task<ForumPost?> GetByIdAsync(Guid id);

    /// <summary>
    /// Update a Post by its GUID.
    /// </summary>
    /// <param name="id">The unique identifier of the post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task UpdateAsync(Guid postId, ForumPost post);

    /// <summary>
    /// Delete an exitsting post by its GUID.
    /// </summary>
    /// <param name="id">The unique identifier of the post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task DeleteAsync(Guid postId);

    Task<bool> TitleExistsAsync(string title);

    Task<bool> isAthorizedAsync(Guid userId, Guid postId);

    Task<bool> PostExistsAsync(Guid postId);

    Task IncrementCommentCountAsync(Guid postId);
}
