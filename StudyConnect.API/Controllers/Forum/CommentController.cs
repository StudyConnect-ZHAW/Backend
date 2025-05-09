using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Models;
using StudyConnect.Core.Interfaces;
using StudyConnect.API.Dtos.Responses.Forum;
using StudyConnect.API.Dtos.Responses.User;

namespace StudyConnect.API.Controllers.Forum;

/// <summary>
/// Controller for managing the comments
/// Provides endpoints to create, retrieve, update, and delete comments.
/// </summary>
[ApiController]
public class CommentController : BaseController
{

    protected readonly ICommentRepository _commentRepository;

    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }
    /// <summary>
    /// Get all comments of a post
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <returns> HTTP 200 OK response on success </returns>
    [Route("v1/posts/{pid}/comments")]
    [HttpGet]
    public async Task<IActionResult> GetAllCommentsOfPost([FromRoute] Guid pid)
    {
        var comments = await _commentRepository.GetAllofPostAsync(pid);
        if (!comments.IsSuccess)
            return BadRequest(comments.ErrorMessage);

        if (comments.Data == null)
            return NotFound("No comments were found");

        var result = comments.Data.Select(c => MapCommentToDtoTree(c));
        return Ok(result);
    }

    /// <summary>
    /// Get comment of a post by its ID
    /// </summary>
    /// <param name="cid"> unique identifier of the comment </param>
    /// <returns> the comment details in JSON </returns>
    [Route("v1/comments/{cid}")]
    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult GetCommentById([FromRoute] Guid cid)
    {
        var mockComment = @"
        { 
                ""CommentId"": ""d2b516f0-d3f5-4a02-8191-5d122c375b2d"",
                ""PostId"": ""d2b876f0-d6h9-4a02-8965-5d248b573j8l"",
                ""Author"": ""John Doe"",
                ""Content"": ""This is a mock content for a mock post."",
                ""MadeAt"": ""2025-03-29T12:34:56""
        }";

        return Ok(mockComment);
    }

    /// <summary>
    /// Creates a new comment
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <returns> HTTP 200 OK response on success </returns>
    [Route("v1/posts/{pid}/comments")]
    [HttpPost]
    public IActionResult CreateComment([FromRoute] Guid pid)
    {
        return Ok();
    }

    /// <summary>
    /// Updates an existing comment
    /// </summary>
    /// <param name="cid"> unique identifier of the comment </param>
    /// <returns> HTTP 200 OK response on success </returns>
    [Route("v1/comments/{cid}")]
    [HttpPut]
    public IActionResult UpdateComment([FromRoute] Guid cid)
    {
        return Ok();
    }

    /// <summary>
    /// Deletes an existing comment
    /// </summary>
    /// <param name="cid"> unique identifier of the comment </param>
    /// <returns> HTTP 200 OK response on success </returns>
    [Route("v1/comments/{cid}")]
    [HttpDelete]
    public IActionResult DeleteComment([FromRoute] Guid cid)
    {
        return Ok();
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

    private CommentReadDto BaseCommentToDto(ForumComment comment)
    {
        return new CommentReadDto
        {
            ForumCommentId = comment.ForumcommentId,
            Content = comment.Content,
            Created = comment.CreatedAt,
            Updated = comment.UpdatedAt,
            Edited = comment.IsEdited,
            Deleted = comment.isDeleted,
            ReplyCount = comment.ReplyCount > 0
                ? comment.ReplyCount
                : null,
            User = comment.User != null
                ? GenerateUserReadDto(comment.User)
                : null,
            Post = comment.Post != null
                ? GeneratePostDto(comment.Post)
                : null,
            ParentCommentId = comment.ParentComment != null
                ? comment.ParentComment.ForumcommentId
                : null
        };
    }

    private CommentReadDto MapCommentToDtoTree(ForumComment comment)
    {
        var commentDto = BaseCommentToDto(comment);
        commentDto.Replies = comment.Replies?
            .Select(c => MapCommentToDtoTree(c))
            .ToList() ?? null;

        return commentDto;
    }
}
