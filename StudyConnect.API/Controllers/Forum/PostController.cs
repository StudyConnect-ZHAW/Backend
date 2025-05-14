using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Interfaces.Services;
using StudyConnect.API.Dtos.Requests.Forum;
using StudyConnect.API.Dtos.Responses.Forum;
using StudyConnect.API.Dtos.Responses.User;
using StudyConnect.Core.Models;
using System.Threading.Tasks;

namespace StudyConnect.API.Controllers.Forum;

/// <summary>
/// Controller for managing the posts
/// Provides endpoints to create, retrieve, update, and delete posts.
/// </summary>
[ApiController]
[Route("api/v1/posts")]
public class PostController : BaseController
{

    protected readonly IPostService _postService;


    public PostController(IPostService postService)
    {
        _postService = postService;
    }


    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="createDto">A Date Transfer Object containing information for post creating.</param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [HttpPost]
    public async Task<IActionResult> AddPost([FromBody] PostCreateDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = new ForumPost
        {
            Title = createDto.Title,
            Content = createDto.Content != null
                ? createDto.Content
                : ""
        };

        var userId = createDto.UserId;
        var categoryId = createDto.ForumCategoryId;

        var result = await _postService.AddPostAsync(userId, categoryId, post);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        if (result.Data == null)
            return NotFound(result.ErrorMessage);

        var output = GeneratePostDto(result.Data);

        return Ok(output);
    }

    /// <summary>
    /// Search posts based on provieded queries.
    /// </summary>
    /// <param name="uid">The unique identifier of the post creator.</param>
    /// <param name="categoryName">The unique name of the category the post belongs to.</param>
    /// <param name="title">the post title or substring of post title</param>
    /// <returns>On success a list of Dtoa with information about the post, on failure HTTP 400/404 status code.</returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchPosts([FromQuery] Guid? uid, [FromQuery] string? categoryName, [FromQuery] string? title, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
    {
        var posts = await _postService.SearchPostAsync(uid, categoryName, title, fromDate, toDate);
        if (!posts.IsSuccess)
            return BadRequest(posts.ErrorMessage);

        if (posts.Data == null)
            return NotFound("Queries not found.");

        var result = posts.Data.Select(p => GeneratePostDto(p));

        return Ok(result);
    }

    /// <summary>
    /// Get a post by its ID
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success a Dto with information about the post, on failure HTTP 400/404 status code.</returns>
    [HttpGet("{pid:guid}")]
    public async Task<IActionResult> GetPostById([FromRoute] Guid pid)
    {
        var post = await _postService.GetPostByIdAsync(pid);
        if (!post.IsSuccess)
            return BadRequest(post.ErrorMessage);

        if (post.Data == null)
            return NotFound("Post not found.");

        var result = GeneratePostDto(post.Data);
        return Ok(result);
    }

    /// <summary>
    /// Update existing post
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <param name="updateDto"> a dto containing the data for updating the post. </param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [HttpPut("{pid:guid}")]
    public async Task<IActionResult> UpdatePost([FromRoute] Guid pid, [FromBody] PostUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = new ForumPost
        {
            Title = updateDto.Title,
            Content = updateDto.Content
        };

        var result = await _postService.UpdatePostAsync(updateDto.UserId, pid, post);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        if (!result.Data)
            return NotFound("Post for update was not found.");

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing post
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [HttpDelete("{pid:guid}")]
    public async Task<IActionResult> DeletePost([FromRoute] Guid pid, [FromQuery] Guid userId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _postService.DeletePostAsync(userId, pid);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        if (!result.Data)
            return NotFound("Post for deletion was not found.");

        return NoContent();
    }


    /// <summary>
    /// A helper function to create user Dto from model.
    /// </summary>
    /// <param name="user">The user model.</param>
    /// <returns>A UserReadDto.</returns>
    private UserReadDto GenerateUserReadDto(User user)
    {
        return new UserReadDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    /// <summary>
    /// A helper function to create category Dto from model.
    /// </summary>
    /// <param name="category">The forum category model.</param>
    /// <returns>A CategoryReadDto.</returns>
    private CategoryReadDto GenerateCategoryReadDto(ForumCategory category)
    {
        return new CategoryReadDto
        {
            ForumCategoryId = category.ForumCategoryId,
            Name = category.Name,
            Description = category.Description
        };
    }

    /// <summary>
    /// A helper function to create post Dto from model.
    /// </summary>
    /// <param name="post">The forum post model.</param>
    /// <returns>A PostReadDto.</returns>
    private PostReadDto GeneratePostDto(ForumPost post)
    {
        return new PostReadDto
        {
            ForumPostId = post.ForumPostId,
            Title = post.Title,
            Content = post.Content,
            Created = post.CreatedAt,
            Updated = post.UpdatedAt,
            Category = post.Category != null
                ? GenerateCategoryReadDto(post.Category)
                : null,
            Author = post.User != null
                ? GenerateUserReadDto(post.User)
                : null
        };
    }
}
