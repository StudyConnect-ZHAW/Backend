using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces.Repositories;

public interface IPostRepository
{
    /// <summary>
    /// Add a Post to the database.
    /// </summary>
    /// <param name="post">The model containing information about the forum post.</param>
    /// <param name="userId">The unique identifier of the user who created the post.</param>
    /// <param name="categoryId">The unique identifier of category the post belogns to.</param>
    /// <returns>The <see cref="Guid"/> of the newly created forum post.</returns>
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

    /// <summary>
    /// Tests if if the title is already taken by another forum post in the database.
    /// </summary>
    /// <param name="title">The title to check for uniqueness.</param>
    /// <returns><c>true</c> if the title exists; otherwise, <c>false</c>.</returns>
    Task<bool> TitleExistsAsync(string title);

    /// <summary>
    /// Tests if the post cotains the provieded user.
    /// </summary>
    /// <param name="userId">The unique idettifier of the user to test for.</param>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns><c>true</c> if the post contains the user; otherwise, <c>false</c>.</returns>
    Task<bool> ContainsUserAsync(Guid userId, Guid postId);

    /// <summary>
    /// Tests if the post exists in the database.
    /// </summary>
    /// <param name="postId">The unique idettifier of the post.</param>
    /// <returns><c>true</c> if the post exists; otherwise, <c>false</c>.</returns>
    Task<bool> ExistsAsync(Guid postId);

    /// <summary>
    /// Increments the comment count of the post.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    Task IncrementCommentCountAsync(Guid postId);
}
