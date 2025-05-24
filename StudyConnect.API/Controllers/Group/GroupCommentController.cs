using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Models;
using StudyConnect.Core.Interfaces;
using StudyConnect.API.Dtos.Responses.Group;
using StudyConnect.API.Dtos.Requests.Group;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.API.Controllers.Groups;

/// <summary>
/// Controller for managing the comments.
/// Provides endpoints to create, retrieve, update, and delete comments.
/// </summary>
[ApiController]
public class GroupCommentController : BaseController
{
    /// <summary>
    /// The comment repository for data operations.
    /// </summary>
    protected readonly IGroupCommentRepository _commentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupCommentController"/> class.
    /// </summary>
    /// <param name="commentRepository">The repository used to manage comment data.</param>
    public GroupCommentController(IGroupCommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="gid">The unique identifier of the post.</param>
    /// <param name="pid"></param>
    /// <param name="createDto">A Data Transfer Object containing information for comment creation.</param>
    /// <returns>Returns HTTP 200 OK on success, or 400 Bad Request on failure.</returns>
    [Route("v1/groups/{gid:guid}/posts/{pid:guid}/comments")]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddGroupComment([FromRoute] Guid gid, [FromRoute] Guid pid, [FromBody] GroupCommentDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = new GroupComment
        {
            Content = createDto.Content
        };

        var uid = GetOIdFromToken();

        var result = await _commentRepository.AddAsync(uid, gid, pid, comment);
        if (!result.IsSuccess || result.Data == null)
            return result.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(result.ErrorMessage)
                : BadRequest(result.ErrorMessage);

        var createdComment = MapToCommentDto(result.Data);

        return Ok(createdComment);
    }

    /// <summary>
    /// Retrieves all comments for a specific post.
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>Returns a list of comments on success, or HTTP 400/404 on failure.</returns>
    [Route("v1/groups/posts/{pid:guid}/comments")]
    [HttpGet]
    public async Task<IActionResult> GetAllCommentsOfPost([FromRoute] Guid pid)
    {
        var comments = await _commentRepository.GetAllofPostAsync(pid);
        if (!comments.IsSuccess || comments.Data == null)
            return comments.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(comments.ErrorMessage)
                : BadRequest(comments.ErrorMessage);

        var result = comments.Data.Select(MapToCommentDto);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a comment by its ID.
    /// </summary>
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <returns>Returns the comment details on success, or HTTP 400/404 on failure.</returns>
    [Route("v1/groups/comments/{cmid:guid}")]
    [HttpGet]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid cmid)
    {
        var comment = await _commentRepository.GetByIdAsync(cmid);
        if (!comment.IsSuccess || comment.Data == null)
            return comment.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(comment.ErrorMessage)
                : BadRequest(comment.ErrorMessage);

        var result = MapToCommentDto(comment.Data);

        return Ok(result);
    }

    /// <summary>
    /// Updates an existing comment.
    /// </summary>
    /// <param name="gid">The unique identifier of group the member belongs to.</param>
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <param name="commentDto">A Data Transfer Object containing updated comment data.</param>
    /// <returns>Returns HTTP 204 No Content on success, or an appropriate error status code on failure.</returns>
    [Route("v1/groups/{gid:guid}/comments/{cmid:guid}")]
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateComment([FromRoute] Guid gid, [FromRoute] Guid cmid, [FromBody] GroupCommentDto commentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = new GroupComment
        {
            Content = commentDto.Content
        };

        var uid = GetOIdFromToken();
        var result = await _commentRepository.UpdateAsync(uid, gid, cmid, comment);
        if (!result.IsSuccess || result.Data == null)
        {
            if (result.ErrorMessage!.Contains(GeneralNotFound)) return NotFound(result.ErrorMessage);
            else if (result.ErrorMessage!.Equals(NotAuthorized)) return Unauthorized(result.ErrorMessage);
            else return BadRequest(result.ErrorMessage);
        }

        return Ok(MapToCommentDto(result.Data));
    }

    /// <summary>
    /// Deletes an existing comment.
    /// </summary>
    /// <param name="gid">the uniq identifier of the group the member belongs to.</param>
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <returns>Returns HTTP 204 No Content on success, or an appropriate error status code on failure.</returns>
    [Route("v1/groups/{gid:guid}/comments/{cmid:guid}")]
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid gid, [FromRoute] Guid cmid)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var uid = GetOIdFromToken();
        var result = await _commentRepository.DeleteAsync(uid, gid, cmid);
        if (!result.IsSuccess)
        {
            if (result.ErrorMessage!.Contains(GeneralNotFound)) return NotFound(result.ErrorMessage);
            else if (result.ErrorMessage!.Equals(NotAuthorized)) return Unauthorized(result.ErrorMessage);
            else return BadRequest(result.ErrorMessage);
        }

        return NoContent();
    }

    private Guid GetOIdFromToken()
    {
        var oidClaim = HttpContext.User.GetObjectId();
        return oidClaim != null
            ? Guid.Parse(oidClaim)
            : Guid.Empty;
    }

    /// <summary>
    /// A helper function to map a GroupMember model to a GroupMemberDto.
    /// </summary>
    /// <param name="member">The group member model.</param>
    /// <returns>A GroupMemberReadDto containing group member details.</returns>
    private GroupMemberReadDto ToMemberDto(GroupMember member)
    {
        return new GroupMemberReadDto
        {
            MemberId = member.MemberId,
            GroupId = member.GroupId,
            JoinedAt = member.JoinedAt,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email
        };
    }

    /// <summary>
    /// A helper function to map a forumComment model to a CommentReadDto.
    /// </summary>
    /// <param name="comment">The comment model.</param>
    /// <returns>A CommentReadDto containing comment details and nested replies, if any.</returns>
    private GroupCommentReadDto MapToCommentDto(GroupComment comment) => new()
    {
        GroupCommentId = comment.GroupCommentId,
        Content = comment.Content,
        Created = comment.CreatedAt,
        Updated = comment.UpdatedAt,
        Edited = comment.IsEdited,
        GroupPostId = comment.GroupPostId,
        Member = ToMemberDto(comment.groupMember!)
    };


}
