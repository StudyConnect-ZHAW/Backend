using Microsoft.AspNetCore.Mvc;
using StudyConnect.Data.Repositories;

namespace StudyConnect.API.Controllers
{
    /// <summary>
    /// APi controller for managing tags.
    /// Provides endpoints for retrieving and managing tags in the system.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly TagRepository _tagRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagController"/> class.
        /// </summary>
        /// <param name="tagRepository">The repository for managing tags.</param>
        public TagController(TagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        /// <summary>
        /// Retrieves all tags from the system.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of tags or a not found result if no tags are available.</returns>
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

        /// <summary>
        /// Placeholder for creating a new tag.
        /// </summary>
        /// <returns>A 501 Not Implemented status code.</returns>
        [HttpPost]
        public IActionResult PostNotImplemented()
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
