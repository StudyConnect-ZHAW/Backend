using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces.Services;

/// <summary>
/// Defines the contract for category-related operations in the forum system.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Retrieves a category by its unique identifier.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<ForumCategory>> GetCategoryByIdAsync(Guid categoryId);

    /// <summary>
    /// Retrieves a category by its name.
    /// </summary>
    /// <param name="name">The name of the category.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>j
    Task<OperationResult<ForumCategory>> GetCategoryByNameAsync(string name);

    /// <summary>
    /// Retrieves all forum categories.
    /// </summary>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<IEnumerable<ForumCategory?>>> GetAllCategoriesAsync();
}

