using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using StudyConnect.API.Dtos.Requests.Group;
using StudyConnect.API.Dtos.Responses.Group;
using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Controllers.Groups
{
    /// <summary>
    /// The group endpoint is used to make modifications to a group.
    /// </summary>
    [ApiController]
    public class GroupController : BaseController
    {
        /// <summary>
        /// The group repository to interact with group data.
        /// </summary>
        protected readonly IGroupRepository _groupRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupController"/> class.
        /// </summary>
        public GroupController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        /// <summary>
        /// Create a new group
        /// </summary>
        [Route("v1/groups")]
        [HttpPost]
        public async Task<IActionResult> AddGroup([FromBody] GroupCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = new Group
            {
                GroupId = Guid.NewGuid(),
                OwnerId = dto.OwnerId,
                Name = dto.Name,
                Description = dto.Description,
            };

            var result = await _groupRepository.AddAsync(group);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return NoContent(); // gleich wie UserController
        }

        /// <summary>
        /// Get a group by id
        /// </summary>
        [Route("v1/groups/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetGroupById([FromRoute] Guid id)
        {
            var result = await _groupRepository.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            if (result.Data == null)
            {
                return NotFound("Group not found.");
            }

            var dto = new GroupReadDto
            {
                GroupId = result.Data.GroupId,
                OwnerId = result.Data.OwnerId,
                Name = result.Data.Name,
                Description = result.Data.Description,
            };

            return Ok(dto);
        }

        /// <summary>
        /// Update a group
        /// </summary>
        [Route("v1/groups/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateGroup([FromRoute] Guid id, [FromBody] GroupUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = new Group
            {
                GroupId = id,
                Name = dto.Name,
                Description = dto.Description,
            };

            var result = await _groupRepository.UpdateAsync(group);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok("Group updated successfully.");
        }

        /// <summary>
        /// Delete a group
        /// </summary>
        [Route("v1/groups/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteGroup([FromRoute] Guid id)
        {
            var result = await _groupRepository.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok("Group deleted successfully.");
        }
    }
}
