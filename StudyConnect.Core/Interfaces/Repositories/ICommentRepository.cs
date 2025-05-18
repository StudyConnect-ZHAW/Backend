using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces.Repositories;


public interface ICommentRepository
{
    /// <summary>
    /// Adds a comment to a post or parent comment.
    /// </summary>
    /// <param name="comment">The model containing information about the comment.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <param name="parentId">The unique identifier of the comment parent.</param>
    /// <returns>The <see cref="Guid"/> of the newly created forum comment.</returns>
    Task<Guid> AddAsync(ForumComment comment, Guid userId, Guid postId, Guid? parentId);

    /// <summary>
    /// Get a comment by its GUID.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <returns>The matching <see cref="ForumComment"/> if found; otherwise, <c>null</c>.</returns>
    Task<ForumComment?> GetByIdAsync(Guid commentId);

    /// <summary>
    /// Retrieves all comments for a specific post.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="ForumComment"/> objects. Can be empty if none exist.</returns>
    Task<IEnumerable<ForumComment>> GetAllofPostAsync(Guid postId);

    /// <summary>
    /// Updates a comment by its GUID.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <param name="userId">The unique identifier of the current user.</param>
    /// <param name="comment">A comment model containing the updated content.</param>
    Task UpdateAsync(Guid commentId, ForumComment comment);

    /// <summary>
    /// Delete a comment by its GUID.
    /// </summary> 
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <param name="userId">The unique identifier of the current user.</param>
    Task DeleteAsync(Guid commentId);

    /// <summary>
    /// Checks whether a comment with the specified ID exists.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <returns><c>true</c> if the comment exists; otherwise, <c>false</c>.</returns>
    Task<bool> ExistsAsync(Guid commentId);

    /// <summary>
    /// Tests if the comment contains the provided post.
    /// </summary>
    /// <param name="postId">The unique identifier of the post to test for.</param>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <returns><c>true</c> if the comment contains the post; otherwise, <c>false</c>.</returns>
    Task<bool> ContainsPostAsync(Guid postId, Guid commentId);

    /// <summary>
    /// Tests if the comment contains the provided user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to test for.</param>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <returns><c>true</c> if the comment contains the user; otherwise, <c>false</c>.</returns>
    Task<bool> ContainsUserAsync(Guid userId, Guid commentId);

    /// <summary>
    /// Increments the reply count of the comment.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment.</param>
    Task IncrementReplyCountAsync(Guid commentId);
}
