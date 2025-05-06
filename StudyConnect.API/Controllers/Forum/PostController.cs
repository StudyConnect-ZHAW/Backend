using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Interfaces;
using StudyConnect.API.Dtos.Requests.Forum;
using StudyConnect.API.Dtos.Responses.Forum;
using StudyConnect.API.Dtos.Responses.User;
using StudyConnect.Core.Models;

namespace StudyConnect.API.Controllers.Forum;

/// <summary>
/// Controller for managing the posts
/// Provides endpoints to create, retrieve, update, and delete posts.
/// </summary>
[ApiController]
[Route("api/v1/posts")]
public class PostController : BaseController
{

    protected readonly IPostRepository _postRepository;


    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    /// <summary>
    /// Creates a new post
    /// </summary>
    /// <returns> HTTP 200 OK response on success </returns>
    [HttpPost]
    public async Task<IActionResult> AddPost([FromBody] PostCreateDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = new ForumPost
        {
            Title = createDto.Title,
            Content = createDto.Content,
        };

        Guid userId = createDto.UserId;
        Guid forumId = createDto.ForumCategoryId;

        var result = await _postRepository.AddAsync(userId, forumId, post);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchPosts([FromQuery] Guid? userId, [FromQuery] string? categoryName, [FromQuery] string? title)
    {
        var posts = await _postRepository.SearchAsync(userId, categoryName, title);
        if (!posts.IsSuccess)
            return BadRequest(posts.ErrorMessage);

        if (posts.Data == null)
            return NotFound("No fitting Queries were found.");

        var result = posts.Data.Select(p => new PostReadDto
        {
            ForumPostId = p.ForumPostId,
            Title = p.Title,
            Content = p.Content,
            Created = p.CreatedAt,
            Updated = p.UpdatedAt,
            Modul = generateCategoryReadDto(p.Category),
            Author = generateUserReadDto(p.User)
        });

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _postRepository.GetAllAsync();
        if (!posts.IsSuccess)
            return BadRequest(posts.ErrorMessage);

        if (posts.Data == null)
            return NotFound("No posts available.");

        var result = posts.Data.Select(p => new PostReadDto
        {
            ForumPostId = p.ForumPostId,
            Title = p.Title,
            Content = p.Content,
            Created = p.CreatedAt,
            Updated = p.UpdatedAt,
            Modul = generateCategoryReadDto(p.Category),
            Author = generateUserReadDto(p.User)
        });

        return Ok(result);
    }

    /// <summary>
    /// Get a post by its ID
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <returns> the post details in JSON </returns>
    [HttpGet("{pid}")]
    public async Task<IActionResult> GetPostById([FromRoute] Guid pid)
    {
        var result = await _postRepository.GetByIdAsync(pid);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        if (result.Data == null)
            return NotFound("Post not found");

        var postDto = new PostReadDto
        {
            ForumPostId = pid,
            Title = result.Data.Title,
            Content = result.Data.Content,
            Created = result.Data.CreatedAt,
            Updated = result.Data.UpdatedAt,
            Modul = generateCategoryReadDto(result.Data.Category),
            Author = generateUserReadDto(result.Data.User)
        };

        return Ok(postDto);
    }

    /// <summary>
    /// Update existing post
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <param name="updateDto"> a dto containing the data for updating the post. </param>
    /// <returns> HTTP 200 OK response on success </returns>
    [HttpPut("{pid}")]
    public async Task<IActionResult> UpdatePost([FromRoute] Guid pid, [FromBody] PostUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = new ForumPost
        {
            Title = updateDto.Title,
            Content = updateDto.Content
        };

        var result = await _postRepository.UpdateAsync(pid, post);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok("post updated successfully.");
    }

    /// <summary>
    /// Deletes an existing post
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <returns> HTTP 200 OK response on success </returns>
    [HttpDelete("{pid}")]
    public async Task<IActionResult> DeletePost([FromRoute] Guid pid)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _postRepository.DeleteAsync(pid);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok("Post deleted successfully.");
    }

    private UserReadDto generateUserReadDto(User? user)
    {
        return new UserReadDto
        {
            FirstName = user?.FirstName,
            LastName = user?.LastName,
            Email = user?.Email
        };
    }

    private CategoryReadDto generateCategoryReadDto(ForumCategory? category)
    {
        return new CategoryReadDto
        {
            ForumCategoryId = category?.ForumCategoryId,
            Name = category?.Name,
            Description = category?.Description
        };
    }
}
