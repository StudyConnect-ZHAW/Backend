using Microsoft.AspNetCore.Mvc;
using StudyConnect.Data.Repositories;

namespace StudyConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly TagRepository _tagRepository;

        public TagController(TagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _tagRepository.GetAllTagsAsync();

            if (tags == null || !tags.Any())
            {
                return NotFound("No tags found.");
            }

            return Ok(tags);
        }

        [HttpPost]
        public IActionResult PostNotImplemented()
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
