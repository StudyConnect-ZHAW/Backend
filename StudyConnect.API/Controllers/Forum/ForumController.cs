using Microsoft.AspNetCore.Mvc;

namespace StudyConnect.API.Controllers.Forum;

[ApiController]
public class ForumController: ControllerBase
{
    [Route("v1/forum")]
    [HttpPost]
    public IActionResult AddDiscussion() 
    {
        return Ok();
    }

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

    [Route("v1/forum/{did}")]
    [HttpPut]
    public IActionResult UpdateDiscussion([FromRoute] Guid did)
    {
        return Ok();
    }

    [Route("v1/forum/{did}")]
    [HttpDelete]
    public IActionResult DeleteDiscussion([FromRoute] Guid did)
    {
        return Ok();
    }

    [Route("v1/forum/{did}/posts")]
    [HttpGet]
    public IActionResult GetAllPostOfDiscussion([FromRoute] Guid did)
    {
        return Ok();
    }

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

    [Route("v1/forum/{did}/posts")]
    [HttpPost]
    public IActionResult CreatePost([FromRoute] Guid did)
    {
        return Ok();
    }

    [Route("v1/forum/{did}/posts{pid}")]
    [HttpPut]
    public IActionResult UpdatePost([FromRoute] Guid did, [FromRoute] Guid pid)
    {
        return Ok();
    }

    [Route("vi/forum/{did}/posts/{pid}")]
    [HttpDelete]
    public IActionResult DeletePost([FromRoute] Guid did, [FromRoute] Guid pid)
    {
        return Ok();
    }
}
