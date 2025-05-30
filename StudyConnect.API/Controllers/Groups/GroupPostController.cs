using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using StudyConnect.API.Dtos.Requests.Group;
using StudyConnect.API.Dtos.Responses.Group;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using static StudyConnect.API.Extensions.MappingExtensions;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.API.Controllers.Groups;

/// <summary>
/// Controller for managing the group post
/// Provides endpoints to create, retrieve, update, and delete posts.
/// </summary>
[ApiController]
public class GroupPostController : BaseController
{
    /// <summary>
    /// The post repository to interact with data.
    /// </summary>
    protected readonly IGroupPostRepository _groupPostRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupPostController"/> class.
    /// </summary>
    /// <param name="groupPostRepository">The post repository to interact with data.</param>
    public GroupPostController(IGroupPostRepository groupPostRepository)
    {
        _groupPostRepository = groupPostRepository;
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
    public async Task<IActionResult> AddGroupPost(
        [FromRoute] Guid gid,
        [FromBody] GroupPostDto createDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = new GroupPost { Title = createDto.Title, Content = createDto.Content };

        Guid uid = GetOIdFromToken();

        var result = await _groupPostRepository.AddAsync(uid, gid, post);
        if (!result.IsSuccess || result.Data == null)
        {
            if (result.ErrorMessage!.Contains(GeneralNotFound))
                return NotFound(result.ErrorMessage);
            else if (result.ErrorMessage!.Contains(GeneralTaken))
                return Conflict(result.ErrorMessage);
            else
                return BadRequest(result.ErrorMessage);
        }

        return Ok(GenerateGroupPostDto(result.Data));
    }

    /// <summary>
    /// Get all the posts.
    /// </summary>
    /// <returns>On success a list of Dtos with information about the post, on failure HTTP 400/404 status code.</returns>
    [Route("v1/groups/{gid:guid}/posts")]
    [HttpGet]
    public async Task<IActionResult> GetAllGroupPosts([FromRoute] Guid gid)
    {
        var posts = await _groupPostRepository.GetAllAsync(gid);
        if (!posts.IsSuccess)
            return BadRequest(posts.ErrorMessage);

        var postsList = posts.Data ?? [];

        var result = postsList.Select(GenerateGroupPostDto);

        return Ok(result);
    }

    /// <summary>
    /// Get a post by its ID.
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success a DTO with information about the post, on failure HTTP 400/404 status code.</returns>
    [Route("v1/groups/posts/{pid:guid}")]
    [HttpGet]
    public async Task<IActionResult> GetGroupPostById([FromRoute] Guid pid)
    {
        var result = await _groupPostRepository.GetByIdAsync(pid);
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
    public async Task<IActionResult> UpdateGroupPost(
        [FromRoute] Guid gid,
        Guid pid,
        [FromBody] GroupPostDto postDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var uid = GetOIdFromToken();

        var post = new GroupPost { Title = postDto.Title, Content = postDto.Content };

        var result = await _groupPostRepository.UpdateAsync(uid, gid, pid, post);
        if (!result.IsSuccess || result.Data == null)
        {
            if (result.ErrorMessage!.Contains(GeneralNotFound))
                return NotFound(result.ErrorMessage);
            else if (result.ErrorMessage!.Equals(NotAuthorized))
                return Unauthorized(result.ErrorMessage);
            else
                return BadRequest(result.ErrorMessage);
        }

        return Ok(GenerateGroupPostDto(result.Data));
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
    public async Task<IActionResult> DeleteGroupPost([FromRoute] Guid gid, [FromRoute] Guid pid)
    {
        var uid = GetOIdFromToken();

        var result = await _groupPostRepository.DeleteAsync(uid, gid, pid);
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
    /// A helper function to create post Dto from model.
    /// </summary>
    /// <param name="post">The forum post model.</param>
    /// <returns>A PostReadDto.</returns>
    private GroupPostReadDto GenerateGroupPostDto(GroupPost post) =>
        new()
        {
            GroupPostId = post.GroupPostId,
            Title = post.Title,
            Content = post.Content,
            Created = post.CreatedAt,
            Updated = post.UpdatedAt,
            CommentCount = post.CommentCount,
            Member = post.GroupMember != null ? post.GroupMember.Member.ToUserReadDto() : null,
        };
}
