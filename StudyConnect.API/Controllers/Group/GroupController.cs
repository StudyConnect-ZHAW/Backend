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
        /// Repository for membership operations.
        /// </summary>
        protected readonly IGroupMemberRepository _groupMemberRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="GroupController"/> class.
        /// </summary>
        public GroupController(IGroupRepository groupRepository, IGroupMemberRepository groupMemberRepository)
        {
            _groupRepository = groupRepository;
            _groupMemberRepository = groupMemberRepository;
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
                CreatedAt = result.Data.CreatedAt
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

            var groups = result.Data ?? [];

            var dtoList = result.Data.Select(g => new GroupReadDto
            {
                GroupId = g.GroupId,
                OwnerId = g.OwnerId,
                Name = g.Name,
                Description = g.Description,
                CreatedAt = g.CreatedAt
            });

            return Ok(dtoList);
        }


        /// <summary>
        /// Adds a user to the specified group.
        /// </summary>
        /// <param name="groupId">Identifier of the target group.</param>
        /// <param name="userId">Identifier of the user who wants to join.</param>
        /// <returns>
        /// 200 OK when the user has been added; 400 Bad Request if the repository returns an error.
        /// </returns>
        [HttpPost]
        [Route("v1/groups/{groupId}/members/{userId}")]
        public async Task<IActionResult> JoinGroup([FromRoute] Guid groupId, [FromRoute] Guid userId)
        {
            var result = await _groupMemberRepository.AddMemberAsync(userId, groupId);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        /// <summary>
        /// Removes a user from the specified group.
        /// </summary>
        /// <param name="groupId">Identifier of the group from which the user should be removed.</param>
        /// <param name="userId">Identifier of the user who wants to leave.</param>
        /// <returns>
        /// 204 No Content when the membership was removed;
        /// 404 Not Found when the membership does not exist;
        /// 400 Bad Request when the repository returns an error.
        /// </returns>
        [HttpDelete]
        [Route("v1/groups/{groupId}/members/{userId}")]
        public async Task<IActionResult> LeaveGroup(
            [FromRoute] Guid groupId,
            [FromRoute] Guid userId)
        {
            var result = await _groupMemberRepository.DeleteMemberAsync(userId, groupId);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            if (!result.Data)
                return NotFound("Membership not foound.");

            return NoContent();
        }

        /// <summary>
        /// Retrieves every member of the given group.
        /// </summary>
        /// <param name="groupId">Identifier of the group whose members are requested.</param>
        /// <returns>
        /// 200 OK with a collection of <see cref="GroupMemberReadDto"/>; 400 Bad Request when the repository reports an error.
        /// </returns>
        [HttpGet]
        [Route("v1/groups/{groupId}/members")]
        public async Task<IActionResult> GetAllMembers([FromRoute] Guid groupId)
        {
            var result = await _groupRepository.GetMembersAsync(groupId);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            var dto = result.Data.Select(m => new GroupMemberReadDto
            {
                MemberId = m.MemberId,
                GroupId = m.GroupId,
                JoinedAt = m.JoinedAt,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email
            });

            return Ok(dto);
        }
    }
}