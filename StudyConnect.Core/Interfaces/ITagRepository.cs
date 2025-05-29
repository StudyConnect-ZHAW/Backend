using StudyConnect.Core.Common;
using StudyConnect.Core.Models;
namespace StudyConnect.Core.Interfaces;

public interface ITagRepository
{
    /// <summary>
    /// Adds a new tag to the database.
    /// </summary>
    /// <param name="tag">The tag to be added.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<Tag>> AddTagAsync(Tag tag);

    /// <summary>
    /// Retrieves all tags from the database.
    /// </summary> 
    /// <returns>An <see cref="OperationResult{T}"/> containing a collection of tags.</returns>
    Task<OperationResult<IEnumerable<Tag>>> GetAllTagsAsync();

    /// <summary>
    /// Retrieves a tag by its unique identifier.
    /// /// </summary>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <returns>An <see cref="OperationResult{T}"/> containing the tag if found, or an error message if not.</returns>
    Task<OperationResult<Tag>> GetTagByIdAsync(Guid tagId);
}
