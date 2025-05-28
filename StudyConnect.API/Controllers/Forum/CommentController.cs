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
/// Controller for managing the comments.
/// Provides endpoints to create, retrieve, update, and delete comments.
/// </summary>
[ApiController]
public class CommentController : BaseController
{
    /// <summary>
    /// The comment repository for data operations.
    /// </summary>
    protected readonly ICommentRepository _commentRepository;

    /// <summary>
    /// The like repository for data operations.
    /// </summary>
    protected readonly ILikeRepository _likeRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentController"/> class.
    /// </summary>
    /// <param name="commentRepository">The repository used to manage comment data.</param>
    /// <param name="likeRepository">The repository used to manage likes data.</param>
    public CommentController(ICommentRepository commentRepository, ILikeRepository likeRepository)
    {
        _commentRepository = commentRepository;
        _likeRepository = likeRepository;
    }

    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <param name="createDto">A Data Transfer Object containing information for comment creation.</param>
    /// <returns>Returns HTTP 200 OK on success, or 400 Bad Request on failure.</returns>
    [Route("v1/posts/{pid:guid}/comments")]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddComment(
        [FromRoute] Guid pid,
        [FromBody] CommentCreateDto createDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = new ForumComment { Content = createDto.Content };

        var uid = GetIdFromToken();
        Guid? ptid = createDto.ParentCommentId;

        var result = await _commentRepository.AddAsync(comment, uid, pid, ptid);
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
    [Route("v1/posts/{pid:guid}/comments")]
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
    [Route("v1/comments/{cmid:guid}")]
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
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <param name="commentDto">A Data Transfer Object containing updated comment data.</param>
    /// <returns>Returns HTTP 204 No Content on success, or an appropriate error status code on failure.</returns>
    [Route("v1/comments/{cmid:guid}")]
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateComment(
        [FromRoute] Guid cmid,
        [FromBody] CommentUpdateDto commentDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = new ForumComment { Content = commentDto.Content };

        var uid = GetIdFromToken();
        var result = await _commentRepository.UpdateAsync(uid, cmid, comment);
        if (!result.IsSuccess || result.Data == null)
        {
            if (result.ErrorMessage!.Contains(GeneralNotFound))
                return NotFound(result.ErrorMessage);
            else if (result.ErrorMessage!.Equals(NotAuthorized))
                return Unauthorized(result.ErrorMessage);
            else
                return BadRequest(result.ErrorMessage);
        }

        return Ok(MapToCommentDto(result.Data));
    }

    /// <summary>
    /// Adds or Removes a like to/from comment.
    /// </summary>
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [Route("v1/comments/{cmid:guid}/likes")]
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> ToggleLike([FromRoute] Guid cmid)
    {
        var uid = GetIdFromToken();

        var result = await _likeRepository.CommentLikeExistsAsync(uid, cmid)
            ? await _likeRepository.UnlikeCommentAsync(uid, cmid)
            : await _likeRepository.LikeCommentAsync(uid, cmid);

        if (!result.IsSuccess && !string.IsNullOrEmpty(result.ErrorMessage))
            return result.ErrorMessage.Contains(GeneralNotFound)
                ? NotFound(result.ErrorMessage)
                : BadRequest(result.ErrorMessage);

        return NoContent();
    }

    /// <summary>
    /// Deletes an existing comment.
    /// </summary>
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <returns>Returns HTTP 204 No Content on success, or an appropriate error status code on failure.</returns>
    [Route("v1/comments/{cmid:guid}")]
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid cmid)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var uid = GetIdFromToken();
        var result = await _commentRepository.DeleteAsync(uid, cmid);
        if (!result.IsSuccess)
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

    private Guid GetIdFromToken()
    {
        var oidClaim = HttpContext.User.GetObjectId();
        return oidClaim != null ? Guid.Parse(oidClaim) : Guid.Empty;
    }

    /// <summary>
    /// A helper function to map a User model to a UserReadDto.
    /// </summary>
    /// <param name="user">The user model.</param>
    /// <returns>A UserReadDto containing user details.</returns>
    private UserReadDto MapUserToDto(User user)
    {
        return new UserReadDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
        };
    }

    /// <summary>
    /// A helper function to map a forumComment model to a CommentReadDto.
    /// </summary>
    /// <param name="comment">The comment model.</param>
    /// <returns>A CommentReadDto containing comment details and nested replies, if any.</returns>
    private CommentReadDto MapToCommentDto(ForumComment comment)
    {
        var result = new CommentReadDto
        {
            ForumCommentId = comment.ForumCommentId,
            Content = comment.Content,
            Created = comment.CreatedAt,
            Updated = comment.UpdatedAt,
            Edited = comment.IsEdited,
            Deleted = comment.IsDeleted,
            ReplyCount = comment.ReplyCount,
            LikeCount = comment.LikeCount,
            UserId = comment.User != null ? comment.User.UserGuid : Guid.Empty,
            User = comment.User != null ? MapUserToDto(comment.User) : null,
            PostId = comment.PostId,
            ParentCommentId = comment.ParentCommentId,
        };

        if (comment.Replies != null && comment.Replies.Count > 0)
        {
            result.Replies = comment.Replies.Select(MapToCommentDto).ToList();
        }

        return result;
    }
}
