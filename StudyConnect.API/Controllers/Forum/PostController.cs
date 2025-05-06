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

    /// <summary>
    /// The post repository to interact with data.
    /// </summary>
    protected readonly IPostRepository _postRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostController"/> class.
    /// </summary>
    /// <param name="postRepository">The post repository to interact with data.</param>
    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
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
            Content = createDto.Content,
        };

        Guid userId = createDto.UserId;
        Guid forumId = createDto.ForumCategoryId;

        var result = await _postRepository.AddAsync(userId, forumId, post);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        var postId = result.Data;

        var locationUri = Url.Action(nameof(GetPostById), new { pid = postId });
        return Created(locationUri, new { pid = postId });
    }

    /// <summary>
    /// Search posts based on provieded queries.
    /// </summary>
    /// <param name="uid">The unique identifier of the post creator.</param>
    /// <param name="categoryName">The unique name of the category the post belongs to.</param>
    /// <param name="title">the post title or substring of post title</param>
    /// <returns>On success a list of Dtoa with information about the post, on failure HTTP 400/404 status code.</returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchPosts([FromQuery] Guid? uid, [FromQuery] string? categoryName, [FromQuery] string? title)
    {
        var posts = await _postRepository.SearchAsync(uid, categoryName, title);
        if (!posts.IsSuccess)
            return BadRequest(posts.ErrorMessage);

        if (posts.Data == null)
            return NotFound("Queries not found.");

        var result = posts.Data.Select(p => GeneratePostDto(p));

        return Ok(result);
    }

    /// <summary>
    /// Get all the posts.
    /// </summary>
    /// <returns>On success a list of Dtos with information about the post, on failure HTTP 400/404 status code.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _postRepository.GetAllAsync();
        if (!posts.IsSuccess)
            return BadRequest(posts.ErrorMessage);

        if (posts.Data == null)
            return NotFound("Posts not found.");

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
        var result = await _postRepository.GetByIdAsync(pid);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        if (result.Data == null)
            return NotFound("Post not found.");

        var postDto = GeneratePostDto(result.Data);

        return Ok(postDto);
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

        var result = await _postRepository.UpdateAsync(pid, post);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        if (result.Data == false)
            return NotFound("Post for update was not found.");

        return Ok("post updated successfully.");
    }

    /// <summary>
    /// Deletes an existing post
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [HttpDelete("{pid:guid}")]
    public async Task<IActionResult> DeletePost([FromRoute] Guid pid)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _postRepository.DeleteAsync(pid);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        if (result.Data == false)
            return NotFound("Post for deletion was not found.");

        return Ok("Post deleted successfully.");
    }

    /// <summary>
    /// A helper function to create user Dto from model.
    /// </summary>
    /// <param name="user">The user model.</param>
    /// <returns>A UserReadDto.</returns>
    private UserReadDto GenerateUserReadDto(User? user)
    {
        return new UserReadDto
        {
            FirstName = user?.FirstName,
            LastName = user?.LastName,
            Email = user?.Email
        };
    }

    /// <summary>
    /// A helper function to create category Dto from model.
    /// </summary>
    /// <param name="category">The forum category model.</param>
    /// <returns>A CategoryReadDto.</returns>
    private CategoryReadDto GenerateCategoryReadDto(ForumCategory? category)
    {
        return new CategoryReadDto
        {
            ForumCategoryId = category?.ForumCategoryId,
            Name = category?.Name,
            Description = category?.Description
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
            Modul = GenerateCategoryReadDto(post.Category),
            Author = GenerateUserReadDto(post.User)
        };
    }
}
