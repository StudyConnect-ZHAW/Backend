using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using StudyConnect.API.Dtos.Requests.Group;
using StudyConnect.API.Dtos.Responses.Group;

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
        /// Creates a new group and returns <c>200 OK</c> if successful.
        /// </summary>
        /// <returns>
        /// 200 OK with the created group on success;
        /// 400 Bad Request if the input is invalid or the repository returns an error.
        /// </returns>
        [Route("v1/groups")]
        [HttpPost]
        public async Task<IActionResult> AddGroup([FromBody] GroupCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newGroup = new Group
            {
                OwnerId = dto.OwnerId,
                Name = dto.Name,
                Description = dto.Description,
            };

            var result = await _groupRepository.AddAsync(newGroup);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            var created = result.Data!;

            var response = new GroupReadDto
            {
                GroupId = created.GroupId,
                OwnerId = created.OwnerId,
                Name = created.Name,
                Description = created.Description,
            };
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a group by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the group to retrieve.</param>
        /// <returns>The group if found; otherwise, a not found or bad request result.</returns>
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
        /// Updates the details of an existing group.
        /// </summary>
        /// <param name="id">The ID of the group to update.</param>
        /// <param name="dto">The updated group data.</param>
        /// <returns>A success message if updated; otherwise, a bad request with an error.</returns>
        [Route("v1/groups/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateGroup(
            [FromRoute] Guid id,
            [FromBody] GroupUpdateDto dto
        )
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

            if (!result.Data)
            {
                return NotFound("Group not found.");
            }

            return Ok("Group updated successfully.");
        }

        /// <summary>
        /// Deletes a group by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the group to delete.</param>
        /// <returns>A success message if deleted; otherwise, a bad request with an error.</returns>
        [Route("v1/groups/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteGroup([FromRoute] Guid id)
        {
            var result = await _groupRepository.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            if (!result.Data)
            {
                return NotFound("Group not found.");
            }

            return NoContent();
        }

        /// <summary>
        /// Retrieves all groups.
        /// </summary>
        /// <returns>A list of groups if found; otherwise, a not found or bad request results.</returns>
        [HttpGet]
        [Route("v1/groups")]
        public async Task<IActionResult> GetAllGroups()
        {
            var result = await _groupRepository.GetAllAsync();

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            var groups = result.Data!;
            if (!groups.Any())
                return NotFound("No groups found.");

            var dtoList = result.Data.Select(g => new GroupReadDto
            {
                GroupId = g.GroupId,
                OwnerId = g.OwnerId,
                Name = g.Name,
                Description = g.Description,
            });

            return Ok(dtoList);
        }
    }
}
