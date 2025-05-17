using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Models;
using StudyConnect.Core.Interfaces.Services;
using StudyConnect.API.Dtos.Responses.Forum;
using StudyConnect.API.Dtos.Requests.Forum;
using StudyConnect.API.Dtos.Responses.User;
using StudyConnect.API.Dtos;
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
    /// Service responsible for handling business logic related to comments. 
    /// </summary>
    protected readonly ICommentService _commentService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentController"/> class.
    /// </summary>
    /// <param name="commentService">
    /// The comment service used to perform business operations and coordinate data access.
    /// </param>
    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <param name="createDto">A DTO containing information for comment creation.</param>
    /// <returns>On success a DTO with information about the created comment, on failure HTTP 400/404 status code.</returns>
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

        var result = await _commentService.AddCommentAsync(comment, userId, pid, parentId);
        if (!result.IsSuccess || result.Data == null)
            return result.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(new ApiResponse<string>(result.ErrorMessage))
                : BadRequest(new ApiResponse<string>(result.ErrorMessage));

        var newComment = ToCommentReadDto(result.Data);

        return Ok(new ApiResponse<CommentReadDto>(newComment));
    }

    /// <summary>
    /// Retrieves all comments for a specific post.
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success a list of DTOs with information about the comments, or HTTP 400/404 on failure.</returns>
    [Route("v1/posts/{pid:guid}/comments")]
    [HttpGet]
    public async Task<IActionResult> GetAllCommentsOfPost([FromRoute] Guid pid)
    {
        var comments = await _commentService.GetAllCommentsOfPostAsync(pid);
        if (!comments.IsSuccess || comments.Data == null)
            return comments.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(new ApiResponse<string>(comments.ErrorMessage))
                : BadRequest(new ApiResponse<string>(comments.ErrorMessage));

        var result = comments.Data!.Select(c => ToCommentReadDto(c!));
        return Ok(new ApiResponse<IEnumerable<CommentReadDto>>(result));
    }

    /// <summary>
    /// Retrieves a comment by its ID.
    /// </summary>
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <returns>On success a DTO with information about the comment, on failure HTTP 400/404 status code.</returns>
    [Route("v1/comments/{cmid:guid}")]
    [HttpGet]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid cmid)
    {
        var comment = await _commentService.GetCommentByIdAsync(cmid);
        if (!comment.IsSuccess || comment.Data == null)
            return comment.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(new ApiResponse<string>(comment.ErrorMessage))
                : BadRequest(new ApiResponse<string>(comment.ErrorMessage));

        var result = ToCommentReadDto(comment.Data);
        return Ok(new ApiResponse<CommentReadDto>(result));
    }

    /// <summary>
    /// Updates an existing comment.
    /// </summary>
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <param name="commentDto">A Data Transfer Object containing updated comment data.</param>
    /// <returns>On success a DTO with information about the updated comment, on failure HTTP 400/404 status code.</returns>
    [Route("v1/comments/{cmid:guid}")]
    [HttpPut]
    public async Task<IActionResult> UpdateComment([FromRoute] Guid cmid, [FromBody] CommentUpdateDto commentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = new ForumComment { Content = commentDto.Content };

        var updateComment = await _commentService.UpdateCommentAsync(cmid, commentDto.UserId, comment);
        if (!updateComment.IsSuccess || updateComment.Data == null)
        {
            if (updateComment.ErrorMessage!.Contains(GeneralNotFound)) return NotFound(new ApiResponse<string>(updateComment.ErrorMessage));
            else if (updateComment.ErrorMessage!.Equals(NotAuthorized)) return Unauthorized(new ApiResponse<string>(updateComment.ErrorMessage));
            else return BadRequest(new ApiResponse<string>(updateComment.ErrorMessage));
        }

        var result = ToCommentReadDto(updateComment.Data);
        return Ok(new ApiResponse<CommentReadDto>(result));
    }

    /// <summary>
    /// Deletes an existing comment.
    /// </summary>
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <param name="userId">The ID of the user requesting the deletion.</param>
    /// <returns> On success HTTP 204 No Content, or an appropriate error status code on failure.</returns>
    [Route("v1/comments/{cmid:guid}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid cmid, [FromQuery] Guid userId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _commentService.DeleteCommentAsync(cmid, userId);
        if (!result.IsSuccess)
        {
            if (result.ErrorMessage!.Contains(GeneralNotFound)) return NotFound(new ApiResponse<string>(result.ErrorMessage));
            else if (result.ErrorMessage!.Equals(NotAuthorized)) return Unauthorized(new ApiResponse<string>(result.ErrorMessage));
            else return BadRequest(new ApiResponse<string>(result.ErrorMessage));
        }

        return NoContent();
    }

    /// <summary>
    /// A helper function to map a User model to a UserReadDto.
    /// </summary>
    /// <param name="user">The user model.</param>
    /// <returns>A UserReadDto containing user details.</returns>
    private UserReadDto ToUserReadDto(User user)
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
    private CommentReadDto ToCommentReadDto(ForumComment comment)
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
                ? ToUserReadDto(comment.User)
                : null,
            PostId = comment.PostId,
            ParentCommentId = comment.ParentCommentId
        };

        if (comment.Replies != null && comment.Replies.Count > 0)
        {
            result.Replies = comment.Replies.Select(c => ToCommentReadDto(c)).ToList();
        }

        return result;
    }
}
