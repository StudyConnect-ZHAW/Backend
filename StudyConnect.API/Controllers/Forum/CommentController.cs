using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Models;
using StudyConnect.Core.Interfaces;
using StudyConnect.API.Dtos.Responses.Forum;
using StudyConnect.API.Dtos.Requests.Forum;
using StudyConnect.API.Dtos.Responses.User;
using System.Threading.Tasks;

namespace StudyConnect.API.Controllers.Forum;

/// <summary>
/// Controller for managing the comments
/// Provides endpoints to create, retrieve, update, and delete comments.
/// </summary>
[ApiController]
public class CommentController : BaseController
{
    /// <summary>
    /// The comment repository to interact wit data.
    /// </summary>
    protected readonly ICommentRepository _commentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentController"/> class.
    /// </summary>
    /// <param name="commentRepository">The comment repository to interact with data.</param>
    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <param name="createDto">A Date Transfer Object containing information for post creating.</param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
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
            return BadRequest(result.ErrorMessage);

        return NoContent();
    }

    /// <summary>
    /// Get all comments of a post.
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success a list of Dtos with information about the comment, on failure a HTTP 400/404 status code.</returns>
    [Route("v1/posts/{pid:guid}/comments")]
    [HttpGet]
    public async Task<IActionResult> GetAllCommentsOfPost([FromRoute] Guid pid)
    {
        var comments = await _commentRepository.GetAllofPostAsync(pid);
        if (!comments.IsSuccess)
            return BadRequest(comments.ErrorMessage);

        if (comments.Data == null)
            return NotFound("No comments were found");

        var result = comments.Data.Select(c => MapCommentToDto(c));
        return Ok(result);
    }

    /// <summary>
    /// Get comment of a post by its ID.
    /// </summary>
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <returns>On success a Dto with information about the comment, on failure a HTTP 400/404 status code.</returns>
    [Route("v1/comments/{cmid:guid}")]
    [HttpGet]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid cmid)
    {
        var comment = await _commentRepository.GetByIdAsync(cmid);
        if (!comment.IsSuccess)
            return BadRequest(comment.ErrorMessage);

        if (comment.Data == null)
            return NotFound("Comment was not found.");

        var result = MapCommentToDto(comment.Data);

        return Ok(result);
    }

    /// <summary>
    /// Updates an existing comment.
    /// </summary>
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <param name="commentDto">A dto conaining the data for updating the comment.</param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [Route("v1/comments/{cmid:guid}")]
    [HttpPut]
    public async Task<IActionResult> UpdateComment([FromRoute] Guid cmid, [FromBody] CommentCreateDto commentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = new ForumComment
        {
            Content = commentDto.Content
        };

        var result = await _commentRepository.UpdateAsync(cmid, commentDto.UserId, comment);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        if (!result.Data)
            return NotFound("Comment for update was not found.");

        return NoContent();
    }

    /// <summary>
    /// Deletes an existing comment.
    /// </summary>
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <param name="userId">The unique identifier of the current user.</param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [Route("v1/comments/{cmid:guid}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid cmid, [FromBody] Guid userId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _commentRepository.DeleteAsync(cmid, userId);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        if (!result.Data)
            return NotFound("Comment for deleting was not found.");

        return NoContent();
    }

    /// <summary>
    /// A helper function to create user Dto from model.
    /// </summary>
    /// <param name="user">The user model.</param>
    /// <returns>A UserReadDto.</returns>
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
    /// A helper function to create comment Dto from model.
    /// </summary>
    /// <param name="comment">The comment model.</param>
    /// <returns>A CommentReadDto.</returns>
    private CommentReadDto MapCommentToDto(ForumComment comment)
    {
        var result = new CommentReadDto
        {
            ForumCommentId = comment.ForumcommentId,
            Content = comment.Content,
            Created = comment.CreatedAt,
            Updated = comment.UpdatedAt,
            Edited = comment.IsEdited,
            Deleted = comment.isDeleted,
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
