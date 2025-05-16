using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Interfaces.Services;
using StudyConnect.API.Dtos.Requests.Forum;
using StudyConnect.API.Dtos.Responses.Forum;
using StudyConnect.API.Dtos;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.API.Controllers.Forum;
/// <summary>
/// Controller for managing the forum categories
/// Provides endpoints to create, retrieve, update, and delete categories.
/// </summary>
[ApiController]
[Route("api/v1/categories")]
public class CategoryController : BaseController
{
    /// <summary>
    /// The category repository to interact with data.
    /// </summary>
    protected readonly ICategoryService _categoryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryController"/> class.
    /// </summary>
    /// <param name="categoryRepository">The category repository to interact with data.</param>
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="categoryDto">A Data Transfer Object containing information for category creating.</param>
    /// <returns>HTTP 501 status code</returns>
    [HttpPost]
    public IActionResult AddCategory([FromBody] CategoryCreateDto categoryDto)
    {
        return StatusCode(501);
    }

    /// <summary>
    /// Get category by id.
    /// </summary>
    /// <param name="cid">The unique identifier of the category.</param>
    /// <returns>On success a Dto with information about the category, on failure HTTP 400/404 status code.</returns>
    [HttpGet("{cid:guid}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid cid)
    {
        var category = await _categoryService.GetCategoryByIdAsync(cid);
        if (!category.IsSuccess || category.Data == null)
            return category.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(new ApiResponse<string>(category.ErrorMessage))
                : BadRequest(new ApiResponse<string>(category.ErrorMessage));

        var categoryDto = new CategoryReadDto
        {
            ForumCategoryId = category.Data.ForumCategoryId,
            Name = category.Data.Name,
            Description = category.Data.Description
        };

        return Ok(new ApiResponse<CategoryReadDto>(categoryDto));
    }

    /// <summary>
    /// Get category by name.
    /// </summary>
    /// <param name="categoryName">The unique name of the category.</param>
    /// <returns>On success a Dto with information about the category, on failure HTTP 400/404 status code.</returns>
    [HttpGet("{categoryName}")]
    public async Task<IActionResult> GetCategoriesByName([FromRoute] string categoryName)
    {
        var category = await _categoryService.GetCategoryByNameAsync(categoryName);
        if (!category.IsSuccess || category.Data == null)
            return category.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(new ApiResponse<string>(category.ErrorMessage))
                : BadRequest(new ApiResponse<string>(category.ErrorMessage));

        var categoryDto = new CategoryReadDto
        {
            ForumCategoryId = category.Data.ForumCategoryId,
            Name = category.Data.Name,
            Description = category.Data.Description
        };

        return Ok(new ApiResponse<CategoryReadDto>(categoryDto));
    }


    /// <summary>
    /// Get all the categories.
    /// </summary>
    /// <returns>On success a list of Dtos with information about the category, on failure HTTP 400/404 status code.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        if (!categories.IsSuccess || categories.Data == null)
            return categories.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(new ApiResponse<string>(categories.ErrorMessage))
                : BadRequest(new ApiResponse<string>(categories.ErrorMessage));


        var result = categories.Data.Select(c => new CategoryReadDto
        {
            ForumCategoryId = c.ForumCategoryId,
            Name = c.Name,
            Description = c.Description
        });

        return Ok(new ApiResponse<IEnumerable<CategoryReadDto>>(result));
    }

    /// <summary>
    /// Delete a category
    /// </summary>
    /// <param name="cid">The unique identifier of the category.</param>
    /// <returns>HTTP 501 status code</returns> 
    [HttpDelete("{cid:guid}")]
    public IActionResult DeleteCategory([FromRoute] Guid cid)
    {
        return StatusCode(501);
    }

}
