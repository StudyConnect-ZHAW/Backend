using StudyConnect.Core.Common;
using StudyConnect.Core.Models;
namespace StudyConnect.Core.Interfaces;


public interface ICategoryRepository
{
    /// <summary>
    /// Add a category to the database
    /// </summary>
    /// <param name="category"> the category model to add </param>
    /// <returns> <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<bool>> AddAsync(ForumCategory? category);

    /// <summary>
    /// Get category by its id
    /// </summary>
    /// <param name="id"> The unique identifier of the category </param>
    /// <returns> <see cref="OperationResult{T}"/> containing the category if found, or an error message if not. </returns>
    Task<OperationResult<ForumCategory?>> GetByIdAsync(Guid id);

    /// <summary>
    /// Get category by its name
    /// </summary>
    /// <param name="name"> The unique name of the category </param>
    /// <returns> <see cref="OperationResult{T}"/> containing the category if found, or an error message if not. </returns>
    Task<OperationResult<ForumCategory?>> GetByNameAsync(string name);

    Task<bool> CategoryExistAsync(Guid categoryId);

    /// <summary>
    /// Get all the categories
    /// </summary>
    /// <returns> <see cref="OperationResult{T}"/> containing  categories if found, or an error message if not. </returns> 
    Task<OperationResult<IEnumerable<ForumCategory>?>> GetAllAsync();

    /// <summary>
    /// Delete a category based on its GUID
    /// </summary>
    /// <param name="guid"> The unique identifier of the category </param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<bool>> DeleteAsync(Guid id);
}

