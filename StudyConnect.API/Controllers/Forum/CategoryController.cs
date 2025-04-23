using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using StudyConnect.API.Dtos.Requests.Forum;
using StudyConnect.API.Dtos.Responses.Forum;

namespace StudyConnect.API.Controllers.Forum;
/// <summary>
/// The category endpoint is used to make modifications to a forumcategory.
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
    /// Add a ForumCategory
    /// </summary>
    /// <param name="categoryDto"> the Category Object to create </param>
    /// <returns>S StatusCode 501 </returns>
    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] CategoryCreateDto categoryDto)
    {
        return StatusCode(501);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        ForumCategory category = new()
        {
            Name = categoryDto.Name,
            Description = categoryDto.Description
        };

        var result = await _categoryRepository.AddAsync(category);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return NoContent();
    }

    /// <summary>
    /// Get category by id
    /// </summary>
    /// <param name="id">the ForumCategory id</param>
    /// <returns> on Success a CatergoryDto, on Failure a BadRequest </returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
    {
        var result = await _categoryRepository.GetByIdAsync(id);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        if (result.Data == null)
            return BadRequest("Category not found.");

        var categoryDto = new CategoryReadDto
        {
            ForumCategoryId = result.Data.ForumCategoryId,
            Name = result.Data.Name,
            Description = result.Data.Description
        };

        return Ok (categoryDto);
    }

    /// <summary>
    /// Get all the categories
    /// </summary>
    /// <returns> on Success a List of CatergoryDtos, on Failure a BadRequest </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllAsync();
        if (!categories.IsSuccess)
            return BadRequest(categories.ErrorMessage);

        if (categories.Data == null)
            return BadRequest("No categories available.");

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
    /// <param name="id"> the unique identifier of the category </param>
    /// <returns> StatusCode 501 </returns> 
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    {
        return StatusCode(501);

        if (id == Guid.Empty)
            return BadRequest("Invalid category ID.");

        var result = await _categoryRepository.DeleteAsync(id);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage); 

        return NoContent();
    }

}
