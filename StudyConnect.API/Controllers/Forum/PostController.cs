using Microsoft.AspNetCore.Mvc;

namespace StudyConnect.API.Controllers.Forum;

/// <summary>
/// Controller for managing the posts
/// Provides endpoints to create, retrieve, update, and delete posts.
/// </summary>
[ApiController]
[Route("v1/forum")]
public class PostController: BaseController
{
    /// <summary>
    /// Creates a new post
    /// </summary>
    /// <returns> HTTP 200 OK response on success </returns>
    [HttpPost]
    public IActionResult AddPost() 
    {
        return Ok();
    }

    /// <summary>
    /// Get a post by its ID
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <returns> the post details in JSON </returns>
    [HttpGet("{pid}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult GetPost([FromRoute] Guid pid ) 
    {
        string mockPost = @"
        { 
            ""PostID"": ""d2b876f0-d6h9-4a02-8965-5d248b573j8l"",
            ""Author"": ""John Doe"",  
            ""Title"": ""Mock Discussion Post Title"",
            ""Tags"": [""C#"", ""API"", ""Mocking""],
            ""MadeAt"": ""2025-03-29T12:34:56"",
            ""Comments"": [
                {
                    ""CommentID"": ""d2b516f0-d3f5-4a02-8191-5d122c375b2d"",
                    ""PostID"": ""d2b876f0-d6h9-4a02-8965-5d248b573j8l"",
                    ""Author"": ""John Doe"",
                    ""Content"": ""This is a mock content for a mock post."",
                    ""MadeAt"": ""2025-03-29T12:34:56""
                }
            ]
        }";

        return Ok(mockPost);
    }

    /// <summary>
    /// Update existing post
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <returns> HTTP 200 OK response on success </returns>
    [HttpPut("{pid}")]
    public IActionResult UpdatePost([FromRoute] Guid pid)
    {
        return Ok();
    }

    /// <summary>
    /// Deletes an existing post
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <returns> HTTP 200 OK response on success </returns>
    [HttpDelete("{pid}")]
    public IActionResult DeletePost([FromRoute] Guid pid)
    {
        return Ok();
    }
}
