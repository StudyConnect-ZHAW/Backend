using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using StudyConnect.API.Dtos.Requests.Forum;
using StudyConnect.API.Dtos.Responses.Forum;

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
    protected readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryController"/> class.
    /// </summary>
    /// <param name="categoryRepository">The category repository to interact with data.</param>
    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
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
    /// <param name="id">The unique identifier of the category.</param>
    /// <returns>On success a Dto with information about the post, on failure HTTP 400/404 status code.</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
    {
        var result = await _categoryRepository.GetByIdAsync(id);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        if (result.Data == null)
            return NotFound("Category not found.");

        var categoryDto = new CategoryReadDto
        {
            ForumCategoryId = result.Data.ForumCategoryId,
            Name = result.Data.Name,
            Description = result.Data.Description
        };

        return Ok (categoryDto);
    }

    /// <summary>
    /// Get all the categories.
    /// </summary>
    /// <returns>On success a list of Dtos with information about the post, on failure HTTP 400/404 status code.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllAsync();
        if (!categories.IsSuccess)
            return BadRequest(categories.ErrorMessage);

        if (categories.Data == null)
            return NotFound("No categories available.");

        var result = categories.Data.Select(c => new CategoryReadDto
        {
            ForumCategoryId = c.ForumCategoryId,
            Name = c.Name,
            Description = c.Description
        });

        return Ok (result);
    }

    /// <summary>
    /// Delete a category
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <returns>HTTP 501 status code</returns> 
    [HttpDelete("{id:guid}")]
    public  IActionResult DeleteCategory([FromRoute] Guid id)
    {
        return StatusCode(501);
     }

}
