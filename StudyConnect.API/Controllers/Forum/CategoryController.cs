using Microsoft.AspNetCore.Mvc;

namespace StudyConnect.API.Controllers.Forum;

[ApiController]
[Route("api/v1/categories")]
public class CategoryController : BaseController
{
    [HttpPost]
    public IActionResult AddCategory()
    {
        return Ok();
    }

    [HttpGet("{cid}")]
    public IActionResult GetCategory()
    {
        return Ok();
    }

    [HttpDelete("{cid}")]
    public IActionResult DeleteCatgory()
    {
        return Ok();
    }

}
