using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Models;
using StudyConnect.Core.Interfaces;
using StudyConnect.API.Dtos.Responses.Forum;
using StudyConnect.API.Dtos.Requests.Forum;
using StudyConnect.API.Dtos.Responses.User;

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
    /// Initializes a new instance of the <see cref="CommentController"/> class.
    /// </summary>
    /// <param name="commentRepository">The repository used to manage comment data.</param>
    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <param name="createDto">A Data Transfer Object containing information for comment creation.</param>
    /// <returns>Returns HTTP 200 OK on success, or 400 Bad Request on failure.</returns>
    [Route("v1/posts/{pid:guid}/comments")]
    [HttpPost]
    public async Task<IActionResult> AddComment([FromRoute] Guid pid, [FromBody] CommentCreateDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = new ForumComment
        {
            Content = createDto.Content
        };

        Guid userId = createDto.UserId;
        Guid? parentId = createDto.ParentCommentId;

        var result = await _commentRepository.AddAsync(comment, userId, pid, parentId);
        if (!result.IsSuccess)
            return result.ErrorMessage!.Contains("not found")
                ? NotFound(result.ErrorMessage)
                : BadRequest(result.ErrorMessage);

        var createdComment = MapCommentToDto(result.Data!);

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
        if (!comments.IsSuccess)
            return comments.ErrorMessage!.Contains("not found")
                ? NotFound(comments.ErrorMessage)
                : BadRequest(comments.ErrorMessage);

        var result = comments.Data!.Select(c => MapCommentToDto(c));
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
        if (!comment.IsSuccess)
            return comment.ErrorMessage!.Contains("not found")
                ? NotFound(comment.ErrorMessage)
                : BadRequest(comment.ErrorMessage);

        var result = MapCommentToDto(comment.Data!);

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
    public async Task<IActionResult> UpdateComment([FromRoute] Guid cmid, [FromBody] CommentUpdateDto commentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = new ForumComment
        {
            Content = commentDto.Content
        };

        var result = await _commentRepository.UpdateAsync(cmid, commentDto.UserId, comment);
        if (!result.IsSuccess)
        {
            if (result.ErrorMessage!.Contains("not found")) return NotFound(result.ErrorMessage);
            else if (result.ErrorMessage!.Contains("authorized")) return Unauthorized(result.ErrorMessage);
            else return BadRequest(result.ErrorMessage);
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes an existing comment.
    /// </summary>
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <param name="userId">The ID of the user requesting the deletion.</param>
    /// <returns>Returns HTTP 204 No Content on success, or an appropriate error status code on failure.</returns>
    [Route("v1/comments/{cmid:guid}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid cmid, [FromQuery] Guid userId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _commentRepository.DeleteAsync(cmid, userId);
        if (!result.IsSuccess)
        {
            if (result.ErrorMessage!.Contains("not found")) return NotFound(result.ErrorMessage);
            else if (result.ErrorMessage!.Contains("authorized")) return Unauthorized(result.ErrorMessage);
            else return BadRequest(result.ErrorMessage);
        }

        return NoContent();
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
            Email = user.Email
        };
    }

    /// <summary>
    /// A helper function to map a forumComment model to a CommentReadDto.
    /// </summary>
    /// <param name="comment">The comment model.</param>
    /// <returns>A CommentReadDto containing comment details and nested replies, if any.</returns>
    private CommentReadDto MapCommentToDto(ForumComment comment)
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
            User = comment.User != null
                ? MapUserToDto(comment.User)
                : null,
            PostId = comment.PostId,
            ParentCommentId = comment.ParentCommentId
        };

        if (comment.Replies != null && comment.Replies.Count > 0)
        {
            result.Replies = comment.Replies.Select(c => MapCommentToDto(c)).ToList();
        }

        return result;
    }
}
