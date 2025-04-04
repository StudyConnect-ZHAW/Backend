using Microsoft.AspNetCore.Mvc;

namespace StudyConnect.API.Controllers.Forum;

/// <summary>
/// Controller for managing the comments
/// Provides endpoints to create, retrieve, update, and delete comments.
/// </summary>
[ApiController]
public class CommitController : BaseController
{
    /// <summary>
    /// Get all comments of a post
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <returns> HTTP 200 OK response on success </returns>
    [Route("v1/fourm/{pid}/comments")]
    [HttpGet]
    public IActionResult GetAllCommentsOfPost([FromRoute] Guid pid)
    {
        return Ok();
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
                ""MadeAt"": ""2025-03-29T12:34:56"",
        }";

        return Ok(mockComment);
    }

    /// <summary>
    /// Creates a new comment
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <returns> HTTP 200 OK response on success </returns>
    [Route("v1/fourm/{pid}/comments")]
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
}