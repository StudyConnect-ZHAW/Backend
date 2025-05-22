using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Interfaces;
using StudyConnect.API.Dtos.Requests.Group;
using StudyConnect.API.Dtos.Responses.Group;
using StudyConnect.API.Dtos.Responses.User;
using StudyConnect.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using static StudyConnect.Core.Common.ErrorMessages;


namespace StudyConnect.API.Controllers.Groups;

/// <summary>
/// Controller for managing the post
/// Provides endpoints to create, retrieve, update, and delete posts.
/// </summary>
[ApiController]
public class PostController : BaseController
{

    /// <summary>
    /// The post repository to interact with data.
    /// </summary>
    protected readonly IGroupPostRepository _postRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostController"/> class.
    /// </summary>
    /// <param name="postRepository">The post repository to interact with data.</param>
    public PostController(IGroupPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="gid">The unique identifier of the group the post belongs to.</param>
    /// <param name="createDto">A Date Transfer Object containing information for post creating.</param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [Route("v1/groups/{gid:guid}/posts")]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddGroupPost([FromRoute] Guid gid, [FromBody] GroupPostDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = new GroupPost
        {
            Title = createDto.Title,
            Content = createDto.Content,
        };

        Guid uid = GetIdFromToken();

        var result = await _postRepository.AddAsync(uid, gid, post);
        if (!result.IsSuccess || result.Data == null)
            return BadRequest(result.ErrorMessage);

        return Ok(GenerateGroupPostDto(result.Data));
    }

    /// <summary>
    /// Get all the posts.
    /// </summary>
    /// <returns>On success a list of Dtos with information about the post, on failure HTTP 400/404 status code.</returns>
    [Route("v1/groups/{gid:guid}/posts")]
    [HttpGet]
    public async Task<IActionResult> GetAllPosts([FromRoute] Guid gid)
    {
        var posts = await _postRepository.GetAllAsync(gid);
        if (!posts.IsSuccess || posts.Data == null)
            return BadRequest(posts.ErrorMessage);

        var result = posts.Data.Select(GenerateGroupPostDto);

        return Ok(result);
    }

    /// <summary>
    /// Get a post by its ID.
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success a DTO with information about the post, on failure HTTP 400/404 status code.</returns>
    [Route("v1/groups/posts/{pid:guid}")]
    [HttpGet]
    public async Task<IActionResult> GetPostById([FromRoute] Guid pid)
    {
        var result = await _postRepository.GetByIdAsync(pid);
        if (!result.IsSuccess || result.Data == null)
            return result.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(result.ErrorMessage)
                : BadRequest(result.ErrorMessage);

        return Ok(GenerateGroupPostDto(result.Data));
    }

    /// <summary>
    /// Update existing post
    /// </summary>
    /// <param name="gid">The unique identifier of group the post belongs to.</param>
    /// <param name="pid"> unique identifier of the post </param>
    /// <param name="postDto"> a dto containing the data for updating the post. </param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [Route("v1/groups/{gid:guid}/posts/{pid:guid}")]
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdatePost([FromRoute] Guid gid, Guid pid, [FromBody] GroupPostDto postDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var uid = GetIdFromToken();

        var post = new GroupPost
        {
            Title = postDto.Title,
            Content = postDto.Content
        };

        var result = await _postRepository.UpdateAsync(uid, gid, pid, post);
        if (!result.IsSuccess)
            return result.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(result.ErrorMessage)
                : BadRequest(result.ErrorMessage);

        return Ok(result.Data);
    }

    /// <summary>
    /// Deletes an existing post.
    /// </summary>
    /// <param name="gid">The unique identifier of group the post belongs to.</param>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success HTTP 204 No Content, or an appropriate error status code on failure.</returns>
    [Route("v1/groups/{gid:guid}/posts/{pid:guid}")]
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePost([FromRoute] Guid gid, [FromRoute] Guid pid)
    {
        var uid = GetIdFromToken();

        var result = await _postRepository.DeleteAsync(uid, gid, pid);
        if (!result.IsSuccess || !result.Data)
            return result.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(result.ErrorMessage)
                : BadRequest(result.ErrorMessage);

        return NoContent();
    }

    private Guid GetIdFromToken()
    {
        var oidClaim = HttpContext.User.GetObjectId();
        return oidClaim != null
            ? Guid.Parse(oidClaim)
            : Guid.Empty;
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
    /// A helper function to create post Dto from model.
    /// </summary>
    /// <param name="post">The forum post model.</param>
    /// <returns>A PostReadDto.</returns>
    private GroupPostReadDto GenerateGroupPostDto(GroupPost post) => new()
    {
        GroupPostId = post.GroupPostId,
        Title = post.Title,
        Content = post.Content,
        Created = post.CreatedAt,
        Updated = post.UpdatedAt,
        GroupId = post.GroupId,
        JoinedAt = post.JoinedAt,
        User = post.User != null
                ? GenerateUserReadDto(post.User)
                : null,
    };
}
