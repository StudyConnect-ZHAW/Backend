using StudyConnect.Core.Models;
namespace StudyConnect.Core.Interfaces;

/// <summary>
/// Defines repository-level data access methods for forum categories.
/// </<summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Adds a new category to the data store.
    /// </summary>
    /// <param name="category">The <see cref="ForumCategory"/> entity to add.</param>
    /// <returns>The <see cref="Guid"/> of the newly created category.</returns>
    Task<Guid> AddAsync(ForumCategory category);

    /// <summary>
    /// Retrieves a category by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <returns>The matching <see cref="ForumCategory"/> if found; otherwise, <c>null</c>.</returns>
    Task<ForumCategory?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a category by its name.
    /// </summary>
    /// <param name="name">The name of the category.</param>
    /// <returns>The matching <see cref="ForumCategory"/> if found; otherwise, <c>null</c>.</returns>
    Task<ForumCategory?> GetByNameAsync(string name);

    /// <summary>
    /// Retrieves all categories from the data store.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="ForumCategory"/> objects. Can be empty if none exist.</returns>
    Task<IEnumerable<ForumCategory?>> GetAllAsync();

    /// <summary>
    /// Deletes the category with the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the category to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Checks whether a category with the specified ID exists.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category.</param>
    /// <returns><c>true</c> if the category exists; otherwise, <c>false</c>.</returns>
    Task<bool> CategoryExistAsync(Guid categoryId);

    /// <summary>
    /// Checks whether the given category name is already in use.
    /// </summary>
    /// <param name="name">The name to check for uniqueness.</param>
    /// <returns><c>true</c> if the name is already taken; otherwise, <c>false</c>.</returns>
    Task<bool> NameTakenAsync(string nakme);

}

