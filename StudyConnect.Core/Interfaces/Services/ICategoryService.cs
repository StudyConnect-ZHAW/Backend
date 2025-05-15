using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces.Services;

public interface ICategoryService
{
    Task<OperationResult<ForumCategory?>> GetCategoryByIdAsync(Guid categoryId);

    Task<OperationResult<ForumCategory?>> GetCategoryByNameAsync(string name);

    Task<OperationResult<IEnumerable<ForumCategory>>> GetAllCategoriesAsync();
}

