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
public class PostController: BaseController
{

    protected readonly IPostRepository _postRepository;


    public PostController (IPostRepository postRepository)
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

        var result = await _postRepository.AddAsync(createDto.UserId, createDto.ForumCategoryId, post);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return NoContent();
    }

    /// <summary>
    /// Filter the Post by Parameters 
    /// </summary>
    /// <param name="category"> the name of ForumCategory </param>
    /// <param name="title"> the title of the Post </param>
    /// <param name="Author"> the creator of the Post </param>
    /// <param name="tags"> a list of Tags for this Post </param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetPostByFilter([FromQuery] string? category, [FromQuery] string? title, [FromQuery] string? Author, [FromQuery] List<string>? tags)
    {
        return Ok("posts");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _postRepository.GetAllAsync();
        if (!posts.IsSuccess)
            return BadRequest(posts.ErrorMessage);

        if (posts.Data == null)
            return NotFound("No posts available.");

        var result = posts.Data.Select(p =>
        {
            var userDto = new UserReadDto
            {
                FirstName = p.User?.FirstName,
                LastName = p.User?.LastName,
                Email = p.User?.Email
            };

            var categoryDto = new CategoryReadDto
            {
                ForumCategoryId = p.Category?.ForumCategoryId,
                Name = p.Category?.Name,
                Description = p.Category?.Description
            };

            return new PostReadDto
            {
               ForumPostId = p.ForumPostId,
               Title = p.Title,
               Content = p.Content,
               Modul = categoryDto,
               Author = userDto
            };
        });

        return Ok (result);
    }

    /// <summary>
    /// Get a post by its ID
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <returns> the post details in JSON </returns>
    [HttpGet("{pid}")]
    public async Task<IActionResult> GetPostById([FromRoute] Guid pid ) 
    {
        var result = await _postRepository.GetByIdAsync(pid);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        if (result.Data == null)
            return NotFound("Post not found");

        var userDto = new UserReadDto
        {
            FirstName = result.Data.User?.FirstName,
            LastName = result.Data.User?.LastName,
            Email = result.Data.User?.Email
        };

        var categoryDto = new CategoryReadDto
        {
            ForumCategoryId = result.Data.Category?.ForumCategoryId,
            Name = result.Data.Category?.Name,
            Description = result.Data.Category?.Description
        };

        var postDto = new PostReadDto
        {
            ForumPostId = pid,
            Title = result.Data.Title,
            Content = result.Data.Content,
            Modul = categoryDto,
            Author = userDto
        };

        return Ok (categoryDto);
    }

    /// <summary>
    /// Update existing post
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <returns> HTTP 200 OK response on success </returns>
    [HttpPut("{pid}")]
    public IActionResult UpdatePost([FromRoute] Guid pid)
    {
        return Ok();
    }

    /// <summary>
    /// Deletes an existing post
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <returns> HTTP 200 OK response on success </returns>
    [HttpDelete("{pid}")]
    public IActionResult DeletePost([FromRoute] Guid pid)
    {
        return Ok();
    }
}
