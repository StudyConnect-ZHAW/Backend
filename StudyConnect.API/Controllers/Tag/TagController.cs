using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyConnect.API.Dtos.Responses;
using StudyConnect.Core.Common;
using StudyConnect.Data.Repositories;

namespace StudyConnect.API.Controllers
{
    /// <summary>
    /// APi controller for managing tags.
    /// Provides endpoints for retrieving and managing tags in the system.
    /// </summary>
    [ApiController]
    [Route("v1/tags")]
    public class TagController : BaseController
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
        //[Authorize]
        public async Task<IActionResult> GetAllTags()
        {
            var result = await _tagRepository.GetAllTagsAsync();

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            var tags = result.Data ?? [];

            var tagsDtoList = tags.Select(t => new TagDto
            {
                Id = t.TagId,
                Name = t.Name,
                Description = t.Description
            });
            return Ok(tagsDtoList);
        }
    }
}
