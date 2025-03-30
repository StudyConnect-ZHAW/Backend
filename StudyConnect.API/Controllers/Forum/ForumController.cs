using Microsoft.AspNetCore.Mvc;

namespace StudyConnect.API.Controllers.Forum;
/// <summary>
/// Controller for managing the forum 
/// Provides endpoints to create, retrieve, update, and delete discussions and posts.
/// </summary>
[ApiController]
public class ForumController: ControllerBase
{
    /// <summary>
    /// Creates a new discussion
    /// </summary>
    /// <return> HTTP 200 OK response on success </return>
    [Route("v1/forum")]
    [HttpPost]
    public IActionResult AddDiscussion() 
    {
        return Ok();
    }

    /// <summary>
    /// Get a discussion by its ID
    /// </summary>
    /// <param name="did"> unique identifier of the discussion </param>
    /// <return> the discussion details in JSON </return>
    [Route("v1/forum/{did}")]
    [HttpGet]
    public IActionResult GetDiscussion([FromRoute] Guid did ) 
    {
        string mockDiscussion = @"
        { 
            ""DiscussionId"": ""d2b876f0-d6h9-4a02-8965-5d248b573j8l"",
            ""Author"": ""John Doe"",  
            ""Title"": ""Mock Discussion Post Title""
            ""Tags"": [""C#"", ""API"", ""Mocking""],
            ""MadeAt"": ""2025-03-29T12:34:56""
            ""Posts"": {
                ""PostId"": ""d2b516f0-d3f5-4a02-8191-5d122c375b2d"",
                ""DiscussionId"": ""d2b876f0-d6h9-4a02-8965-5d248b573j8l"",
                ""Author"": ""John Doe"",
                ""Content"": ""This is a mock content for a mock post."",
                ""MadeAt"": ""2025-03-29T12:34:56""
            },
        }";

        return Ok(mockDiscussion);
    }

    /// <summary>
    /// Update existing discussion
    /// </summary>
    /// <param name="did"> unique identifier of the discussion </param>
    /// <return> HTTP 200 OK response on success </return>
    [Route("v1/forum/{did}")]
    [HttpPut]
    public IActionResult UpdateDiscussion([FromRoute] Guid did)
    {
        return Ok();
    }

    /// <summary>
    /// Deletes an existing discussion
    /// </summary>
    /// <param name="did"> unique identifier of the discussion </param>
    /// <return> HTTP 200 OK response on success </return>
    [Route("v1/forum/{did}")]
    [HttpDelete]
    public IActionResult DeleteDiscussion([FromRoute] Guid did)
    {
        return Ok();
    }

    /// <summary>
    /// Get all posts of a discussion
    /// </summary>
    /// <param name="did"> unique identifier of the discussion </param>
    /// <return> HTTP 200 OK response on success </return>
    [Route("v1/forum/{did}/posts")]
    [HttpGet]
    public IActionResult GetAllPostOfDiscussion([FromRoute] Guid did)
    {
        return Ok();
    }

    /// <summary>
    /// Get post of a discussion by its ID
    /// </summary>
    /// <param name="did"> unique identifier of the discussion </param>
    /// <param name="pid"> unique identifier of the post </param>
    /// <return> the post details in JSON </return>
    [Route("v1/forum/{did}/posts/{pid}")]
    [HttpGet]
    public IActionResult GetPostById([FromRoute] Guid did, [FromRoute] Guid pid)
    {
        var mockPost = @"
        {
                ""PostId"": ""d2b516f0-d3f5-4a02-8191-5d122c375b2d"",
                ""DiscussionId"": ""d2b876f0-d6h9-4a02-8965-5d248b573j8l"",
                ""Author"": ""John Doe"",
                ""Content"": ""This is a mock content for a mock post."",
                ""MadeAt"": ""2025-03-29T12:34:56""
        }";

        return Ok(mockPost);
    }

    /// <summary>
    /// Creates a new post
    /// </summary>
    /// <param name="did"> unique identifier of the discussion </param>
    /// <return> HTTP 200 OK response on success </return>
    [Route("v1/forum/{did}/posts")]
    [HttpPost]
    public IActionResult CreatePost([FromRoute] Guid did)
    {
        return Ok();
    }

    /// <summary>
    /// Updates an existing post
    /// </summary>
    /// <param name="did"> unique identifier of the discussion </param>
    /// <param name="pid"> unique inheritdoc of the posts </param>
    /// <return> HTTP 200 OK response on success </return>
    [Route("v1/forum/{did}/posts{pid}")]
    [HttpPut]
    public IActionResult UpdatePost([FromRoute] Guid did, [FromRoute] Guid pid)
    {
        return Ok();
    }

    /// <summary>
    /// Deletes an existing post
    /// </summary>
    /// <param name="did"> unique identifier of the discussion </param>
    /// <param name="pid"> unique inheritdoc of the posts </param>
    /// <return> HTTP 200 OK response on success </return>
    [Route("v1/forum/{did}/posts/{pid}")]
    [HttpDelete]
    public IActionResult DeletePost([FromRoute] Guid did, [FromRoute] Guid pid)
    {
        return Ok();
    }
}
