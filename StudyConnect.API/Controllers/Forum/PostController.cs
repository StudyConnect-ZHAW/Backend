using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Interfaces;
using StudyConnect.API.Dtos.Requests.Forum;
using StudyConnect.API.Dtos.Responses.Forum;
using StudyConnect.API.Dtos.Responses.User;
using StudyConnect.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using static StudyConnect.Core.Common.ErrorMessages;


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

    protected readonly ILikeRepository _likeRepository;


    /// <summary>
    /// Initializes a new instance of the <see cref="PostController"/> class.
    /// </summary>
    /// <param name="postRepository">The post repository to interact with data.</param>
    public PostController(IPostRepository postRepository, ILikeRepository likeRepository)
    {
        _postRepository = postRepository;
        _likeRepository = likeRepository;
    }

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="createDto">A Date Transfer Object containing information for post creating.</param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPost([FromBody] PostCreateDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = new ForumPost
        {
            Title = createDto.Title,
            Content = createDto.Content,
        };

        var oidClaim = HttpContext.User.GetObjectId();
        if (string.IsNullOrEmpty(oidClaim))
            return Unauthorized();

        Guid userId = Guid.Parse(oidClaim);
        Guid forumId = createDto.ForumCategoryId;

        var result = await _postRepository.AddAsync(userId, forumId, post);
        if (!result.IsSuccess || result.Data == null)
            return BadRequest(result.ErrorMessage);

        return Ok(GeneratePostDto(result.Data));
    }

    // [HttpPost("{pid:guid}/likes")]
    // [Authorize]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // public async Task<IActionResult> AddLike([FromBody] Guid pid)
    // {
    //     var oidClaim = HttpContext.User.GetObjectId();
    //     if (string.IsNullOrEmpty(oidClaim))
    //         return Unauthorized();
    //
    //     var uid = Guid.Parse(oidClaim);
    //
    //     var result = await _likeRepository.LikePostAsync(uid, pid);
    //     if (!result.IsSuccess || !result.Data)
    //         return result.ErrorMessage!.Contains(GeneralNotFound)
    //             ? NotFound(result.ErrorMessage)
    //             : BadRequest(result.ErrorMessage);
    //
    //     return NoContent();
    // }

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
    [Authorize]
    public async Task<IActionResult> UpdatePost([FromRoute] Guid pid, [FromBody] PostUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var oidClaim = HttpContext.User.GetObjectId();
        if (string.IsNullOrEmpty(oidClaim))
            return Unauthorized();


        var post = new ForumPost
        {
            Title = updateDto.Title,
            Content = updateDto.Content
        };

        var result = await _postRepository.UpdateAsync(Guid.Parse(oidClaim), pid, post);
        if (!result.IsSuccess || result.Data == null)
            return result.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(result.ErrorMessage)
                : BadRequest(result.ErrorMessage);

        return Ok(result.Data);
    }

    [HttpPut("{pid:guid}/likes")]
    [Authorize]
    public async Task<IActionResult> ToggleLike([FromRoute] Guid pid)
    {

        var oidClaim = HttpContext.User.GetObjectId();
        var uid = oidClaim != null
            ? Guid.Parse(oidClaim)
            : Guid.Empty;


        var result = await _likeRepository.PostLikeExistsAsync(uid, pid)
            ? await _likeRepository.UnlikePostAsync(uid, pid)
            : await _likeRepository.LikePostAsync(uid, pid);


        if (!result.IsSuccess && !string.IsNullOrEmpty(result.ErrorMessage))
            return result.ErrorMessage.Contains(GeneralNotFound)
                ? NotFound(result.ErrorMessage)
                : BadRequest(result.ErrorMessage);

        return NoContent();
    }

    /// <summary>
    /// Deletes an existing post
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [HttpDelete("{pid:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePost([FromRoute] Guid pid)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var oidClaim = HttpContext.User.GetObjectId();
        if (string.IsNullOrEmpty(oidClaim))
            return Unauthorized();

        var result = await _postRepository.DeleteAsync(Guid.Parse(oidClaim), pid);
        if (!result.IsSuccess || !result.Data)
            return result.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(result.ErrorMessage)
                : BadRequest(result.ErrorMessage);

        return NoContent();
    }

    // [HttpDelete("{pid: guid}/likes")]
    // [Authorize]
    // public async Task<IActionResult> DeleteLike([FromBody] Guid pid)
    // {
    //     var oidClaim = HttpContext.User.GetObjectId();
    //     if (string.IsNullOrEmpty(oidClaim))
    //         return Unauthorized();
    //
    //     var uid = Guid.Parse(oidClaim);
    //
    //     var result = await _likeRepository.UnlikePostAsync(uid, pid);
    //     if (!result.IsSuccess || !result.Data)
    //         return result.ErrorMessage!.Contains(GeneralNotFound)
    //             ? NotFound(result.ErrorMessage)
    //             : BadRequest(result.ErrorMessage);
    //
    //     return NoContent();
    // }

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
