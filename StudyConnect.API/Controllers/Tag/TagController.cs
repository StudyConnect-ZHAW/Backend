using Microsoft.AspNetCore.Mvc;
using StudyConnect.Data.Repositories;
using StudyConnect.API.Dtos.Responses;

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

            var tagDtos = tags.Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description
            });

            return Ok(tagDtos);
        }

        [HttpPost]
        public IActionResult PostNotImplemented()
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
