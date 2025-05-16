using StudyConnect.Core.Interfaces.Services;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Common;
using StudyConnect.Core.Models;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.Services;

public class CategoryService : ICategoryService
{
    protected readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<OperationResult<IEnumerable<ForumCategory?>>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        if (categories == null || !categories.Any())
            return OperationResult<IEnumerable<ForumCategory?>>.Success(new List<ForumCategory>());

        return OperationResult<IEnumerable<ForumCategory?>>.Success(categories);
    }

    public async Task<OperationResult<ForumCategory?>> GetCategoryByIdAsync(Guid categoryId)
    {
        if (IsInvalid(categoryId))
            return OperationResult<ForumCategory?>.Failure(InvalidCategoryId);

        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
            return OperationResult<ForumCategory?>.Failure(CategoryNotFound);

        return OperationResult<ForumCategory?>.Success(category);
    }

    public async Task<OperationResult<ForumCategory?>> GetCategoryByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return OperationResult<ForumCategory?>.Failure(NameRequired);

        var category = await _categoryRepository.GetByNameAsync(name);
        if (category == null)
            return OperationResult<ForumCategory?>.Failure(CategoryNotFound);

        return OperationResult<ForumCategory?>.Success(category);
    }

    private static bool IsInvalid(Guid id) => id == Guid.Empty;

}
