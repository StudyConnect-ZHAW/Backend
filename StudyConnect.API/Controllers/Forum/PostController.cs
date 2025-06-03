using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using StudyConnect.API.Dtos.Requests.Forum;
using StudyConnect.API.Dtos.Responses.Forum;
using StudyConnect.API.Dtos.Responses.User;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
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

    /// <summary>
    /// The like repository to interact with data.
    /// </summary>
    protected readonly ILikeRepository _likeRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostController"/> class.
    /// </summary>
    /// <param name="postRepository">The post repository to interact with data.</param>
    /// <param name="likeRepository">The like repository to interact with data.</param>
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

        var post = new ForumPost { Title = createDto.Title, Content = createDto.Content };

        Guid uid = GetOIdFromToken();
        Guid pid = createDto.ForumCategoryId;

        var result = await _postRepository.AddAsync(uid, pid, post);
        if (!result.IsSuccess || result.Data == null)
        {
            if (result.ErrorMessage!.Contains(GeneralNotFound))
                return NotFound(result.ErrorMessage);
            else
                return BadRequest(result.ErrorMessage);
        }
        return Ok(GeneratePostDto(result.Data));
    }

    /// <summary>
    /// Search posts based on provieded queries.
    /// </summary>
    /// <param name="uid">The unique identifier of the post creator.</param>
    /// <param name="categoryName">The unique name of the category the post belongs to.</param>
    /// <param name="title">the post title or substring of post title</param>
    /// <param name="fromDate">The start date to filter posts created on or after this date (optional).</param>
    /// <param name="toDate">The end date to filter posts created on or before this date (optional).</param>
    /// <returns>On success a list of Dtoa with information about the post, on failure HTTP 400/404 status code.</returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchPosts(
        [FromQuery] Guid? uid,
        [FromQuery] string? categoryName,
        [FromQuery] string? title,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate
    )
    {
        var posts = await _postRepository.SearchAsync(uid, categoryName, title, fromDate, toDate);
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

        var postsList = posts.Data ?? [];

        var result = postsList.Select(p => GeneratePostDto(p));

        return Ok(result);
    }

    /// <summary>
    /// Get a post by its ID.
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success a DTO with information about the post, on failure HTTP 400/404 status code.</returns>
    [HttpGet("{pid:guid}")]
    public async Task<IActionResult> GetPostById([FromRoute] Guid pid)
    {
        var result = await _postRepository.GetByIdAsync(pid);
        if (!result.IsSuccess || result.Data == null)
            return result.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(result.ErrorMessage)
                : BadRequest(result.ErrorMessage);

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
    public async Task<IActionResult> UpdatePost(
        [FromRoute] Guid pid,
        [FromBody] PostUpdateDto updateDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var uid = GetOIdFromToken();

        var post = new ForumPost { Title = updateDto.Title, Content = updateDto.Content };

        var result = await _postRepository.UpdateAsync(uid, pid, post);
        if (!result.IsSuccess || result.Data == null)
        {
            if (result.ErrorMessage!.Contains(GeneralNotFound))
                return NotFound(result.ErrorMessage);
            else if (result.ErrorMessage!.Equals(NotAuthorized))
                return Unauthorized(result.ErrorMessage);
            else
                return BadRequest(result.ErrorMessage);
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Adds or Removes a like to/from post.
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [HttpPut("{pid:guid}/likes")]
    [Authorize]
    public async Task<IActionResult> ToggleLike([FromRoute] Guid pid)
    {
        var uid = GetOIdFromToken();

        var result = await _likeRepository.PostLikeExistsAsync(uid, pid)
            ? await _likeRepository.UnlikePostAsync(uid, pid)
            : await _likeRepository.LikePostAsync(uid, pid);

        if (!result.IsSuccess && !string.IsNullOrEmpty(result.ErrorMessage))
            return result.ErrorMessage.Contains(GeneralNotFound)
                ? NotFound(result.ErrorMessage)
                : BadRequest(result.ErrorMessage);

        var dto = new ToggleLikeDto { AddedLike = result.Data };

        return Ok(dto);
    }

    /// <summary>
    /// Get all likes for the current user.
    /// </summary>
    /// <returns>On success a HTTP 200 status code, or an appropriate error status code on failure.</returns>
    [HttpGet("likes")]
    [Authorize]
    public async Task<IActionResult> GetPostLikesForCurrentUser()
    {
        var uid = GetIdFromToken();

        var likes = await _likeRepository.GetPostLikesForUser(uid);

        if (!likes.IsSuccess && !string.IsNullOrEmpty(likes.ErrorMessage))
            return likes.ErrorMessage.Contains(GeneralNotFound)
                ? NotFound(likes.ErrorMessage)
                : BadRequest(likes.ErrorMessage);

        var likesList = likes.Data ?? [];

        return Ok(likesList.Select(ToPostLikeDto));
    }

    /// <summary>
    /// Deletes an existing post.
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success HTTP 204 No Content, or an appropriate error status code on failure.</returns>
    [HttpDelete("{pid:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePost([FromRoute] Guid pid)
    {
        var uid = GetOIdFromToken();

        var result = await _postRepository.DeleteAsync(uid, pid);
        if (!result.IsSuccess || !result.Data)
        {
            if (result.ErrorMessage!.Contains(GeneralNotFound))
                return NotFound(result.ErrorMessage);
            else if (result.ErrorMessage!.Equals(NotAuthorized))
                return Unauthorized(result.ErrorMessage);
            else
                return BadRequest(result.ErrorMessage);
        }
        return NoContent();
    }

    private Guid GetOIdFromToken()
    {
        var oidClaim = HttpContext.User.GetObjectId();
        return oidClaim != null ? Guid.Parse(oidClaim) : Guid.Empty;
    }

    /// <summary>
    /// A helper function to create user Dto from model.
    /// </summary>
    /// <param name="user">The user model.</param>
    /// <returns>A UserReadDto.</returns>
    private UserReadDto GenerateUserReadDto(User user) =>
        new()
        {
            Oid = user.UserGuid,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
        };

    /// <summary>
    /// A helper function to create category Dto from model.
    /// </summary>
    /// <param name="category">The forum category model.</param>
    /// <returns>A CategoryReadDto.</returns>
    private CategoryReadDto GenerateCategoryReadDto(ForumCategory category) =>
        new()
        {
            ForumCategoryId = category.ForumCategoryId,
            Name = category.Name,
            Description = category.Description,
        };

    /// <summary>
    /// A helper function to create postlike Dto from model.
    /// </summary>
    /// <param name="like">The forum like model.</param>
    /// <returns>A PostLikeReadDto.</returns>
    private PostLikeReadDto ToPostLikeDto(ForumLike like) =>
        new()
        {
            LikeId = like.LikeId,
            UserId = like.UserId,
            ForumPostId = like.ForumPostId,
            LikedAt = like.LikedAt,
        };

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
            CommentCount = post.CommentCount,
            LikeCount = post.LikeCount,
            Category = post.Category != null ? GenerateCategoryReadDto(post.Category) : null,
            User = post.User != null ? GenerateUserReadDto(post.User) : null,
        };
    }
}
