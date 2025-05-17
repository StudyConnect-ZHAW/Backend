using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces.Services;

/// <summary>
/// Defines the contract for post-related operations in the forum system.
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Adds a new forum post to the database.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who created the post.</param>
    /// <param name="categoryId">The unique identifier of category the post belogns to.</param>
    /// <param name="post">The model containing information about the forum post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<ForumPost>> AddPostAsync(Guid userId, Guid categoryId, ForumPost post);

    /// <summary>
    /// Searches the database for forum posts based on the provided query parameters. 
    /// </summary>
    /// <param name="userId">The unique identifier of the post creator (optional).</param>
    /// <param name="categoryName">The name of the category the post belongs to (optional).</param>
    /// <param name="title">The title or part of the title to search for (optional).</param>
    /// <param name="fromDate">The start date to filter posts created on or after this date (optional).</param>
    /// <param name="toDate">The end date to filter posts created on or before this date (optional).</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<IEnumerable<ForumPost>>> SearchPostAsync(
          Guid? userId,
          string? categoryName,
          string? title,
          DateTime? fromDate,
          DateTime? toDate
    );

    /// <summary>
    /// Retrieves a post by its unique identifier.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<ForumPost>> GetPostByIdAsync(Guid postId);

    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who created the post.</param>
    /// <param name="postId">The unique identifier of the post, that shoudl be updated.</param>
    /// <param name="post">The model containing the information about the forum post for the update.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<ForumPost>> UpdatePostAsync(Guid userId, Guid postId, ForumPost post);

    /// <summary>
    /// Deletes an exsiting post.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who created the post.</param>
    /// <param name="postId">The unique identifier of the post, that shoudl be deleted.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<bool>> DeletePostAsync(Guid userId, Guid postId);
}
