using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces.Repositories;

public interface ICommentRepository
{
    /// <summary>
    /// Adds an comment to a post or parent comment.
    /// </summary>
    /// <param name="comment">The model containing information about the comment.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <param name="parentId">The unique identifier of the comment parent.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<Guid> AddAsync(ForumComment comment, Guid userId, Guid postId, Guid? parentId);

    /// <summary>
    /// Get a comment by its GUID.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<ForumComment?> GetByIdAsync(Guid commentId);

    /// <summary>
    /// Retrieves all comments for a specific post.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<IEnumerable<ForumComment>?> GetAllofPostAsync(Guid postId);

    /// <summary>
    /// Updates a comment by its GUID.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <param name="userId">The unique identifier of the current user.</param>
    /// <param name="comment">A comment model containing the updated content.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task UpdateAsync(Guid commentId, ForumComment comment);

    /// <summary>
    /// Delete a comment by its GUID.
    /// </summary> 
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <param name="userId">The unique identifier of the current user.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task DeleteAsync(Guid commentId);

    Task<bool> CommentExistsAsync(Guid commentId);

    Task<bool> isChildOfPostAsync(Guid postId, Guid commentId);

    Task<bool> isAuthorizedAsync(Guid userId, Guid commentId);

    Task IncrementReplyCountAsync(Guid commentId);
}
