using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using StudyConnect.API.Dtos.Requests.Group;
using StudyConnect.API.Dtos.Responses.Group;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using static StudyConnect.Core.Common.ErrorMessages;

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
        public GroupController(
            IGroupRepository groupRepository,
            IGroupMemberRepository groupMemberRepository
        )
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
        [Authorize]
        public async Task<IActionResult> AddGroup([FromBody] GroupCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newGroup = new Group
            {
                OwnerId = GetOIdFromToken(),
                Name = dto.Name,
                Description = dto.Description,
            };

            var result = await _groupRepository.AddAsync(newGroup);
            if (!result.IsSuccess || result.Data == null)
            {
                if (result.ErrorMessage!.Contains(GeneralNotFound))
                    return NotFound(result.ErrorMessage);
                else if (result.ErrorMessage!.Contains(GeneralTaken))
                    return Conflict(result.ErrorMessage);
                else
                    return BadRequest(result.ErrorMessage);
            }

            var response = ToGroupReadDto(result.Data);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a group by its unique identifier.
        /// </summary>
        /// <param name="groupId">The ID of the group to retrieve.</param>
        /// <returns>The group if found; otherwise, a not found or bad request result.</returns>
        [Route("v1/groups/{groupId:guid}")]
        [HttpGet]
        public async Task<IActionResult> GetGroupById([FromRoute] Guid groupId)
        {
            var result = await _groupRepository.GetByIdAsync(groupId);

            if (!result.IsSuccess || result.Data == null)
                return result.ErrorMessage!.Contains(GeneralNotFound)
                    ? NotFound(result.ErrorMessage)
                    : BadRequest(result.ErrorMessage);

            return Ok(ToGroupReadDto(result.Data));
        }

        /// <summary>
        /// Updates the details of an existing group.
        /// </summary>
        /// <param name="groupId">The ID of the group to update.</param>
        /// <param name="dto">The updated group data.</param>
        /// <returns>A success message if updated; otherwise, a bad request with an error.</returns>
        [Route("v1/groups/{groupId:guid}")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateGroup(
            [FromRoute] Guid groupId,
            [FromBody] GroupUpdateDto dto
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = new Group
            {
                GroupId = groupId,
                OwnerId = GetOIdFromToken(),
                Name = dto.Name,
                Description = dto.Description,
            };

            var result = await _groupRepository.UpdateAsync(group);

            if (!result.IsSuccess || result.Data == null)
            {
                if (result.ErrorMessage!.Contains(GeneralNotFound))
                    return NotFound(result.ErrorMessage);
                else if (result.ErrorMessage!.Equals(NotAuthorized))
                    return Unauthorized(result.ErrorMessage);
                else
                    return BadRequest(result.ErrorMessage);
            }

            return Ok(ToGroupReadDto(result.Data));
        }

        /// <summary>
        /// Deletes a group by its unique identifier.
        /// </summary>
        /// <param name="groupId">The ID of the group to delete.</param>
        /// <returns>A success message if deleted; otherwise, a bad request with an error.</returns>
        [Route("v1/groups/{groupId:guid}")]
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteGroup([FromRoute] Guid groupId)
        {
            var userId = GetOIdFromToken();
            var result = await _groupRepository.DeleteAsync(userId, groupId);

            if (!result.IsSuccess || !result.Data)
            {
                if (result.ErrorMessage!.Contains(GeneralNotFound))
                    return NotFound(result.ErrorMessage);
                else if (result.ErrorMessage!.Equals(NotAuthorized))
                    return Unauthorized(result.ErrorMessage);
                else
                    return BadRequest(result.ErrorMessage);
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

            var dtoList = groups.Select(ToGroupReadDto).ToList();

            return Ok(dtoList);
        }

        /// <summary>
        /// Adds a user to the specified group.
        /// </summary>
        /// <param name="groupId">Identifier of the target group.</param>
        /// <returns>
        /// 200 OK when the user has been added; 400 Bad Request if the repository returns an error.
        /// </returns>
        [HttpPost]
        [Route("v1/groups/{groupId}/members")]
        [Authorize]
        public async Task<IActionResult> JoinGroup([FromRoute] Guid groupId)
        {
            var userId = GetOIdFromToken();
            var result = await _groupMemberRepository.AddMemberAsync(userId, groupId);

             if (!result.IsSuccess || result.Data == null)
            {
                if (result.ErrorMessage!.Contains(GeneralNotFound))
                    return NotFound(result.ErrorMessage);
                else if (result.ErrorMessage!.Contains(GeneralTaken))
                    return Conflict(result.ErrorMessage);
                else
                    return BadRequest(result.ErrorMessage);
            }
            var resultDto = new GroupMemberReadDto
            {
                GroupId = result.Data.GroupId,
                MemberId = result.Data.MemberId,
                JoinedAt = result.Data.JoinedAt,
                FirstName = result.Data.FirstName,
                LastName = result.Data.LastName,
                Email = result.Data.Email,
            };

            return Ok(resultDto);
        }

        /// <summary>
        /// Removes a user from the specified group.
        /// </summary>
        /// <param name="groupId">Identifier of the group from which the user should be removed.</param>
        /// <returns>
        /// 204 No Content when the membership was removed;
        /// 404 Not Found when the membership does not exist;
        /// 400 Bad Request when the repository returns an error.
        /// </returns>
        [HttpDelete]
        [Route("v1/groups/{groupId}/members")]
        [Authorize]
        public async Task<IActionResult> LeaveGroup([FromRoute] Guid groupId)
        {
            var userId = GetOIdFromToken();
            var result = await _groupMemberRepository.DeleteMemberAsync(userId, groupId);

             if (!result.IsSuccess || !result.Data)
                return result.ErrorMessage!.Contains(GeneralNotFound)
                    ? NotFound(result.ErrorMessage)
                    : BadRequest(result.ErrorMessage);

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
        [Route("v1/groups/{groupId:guid}/members")]
        public async Task<IActionResult> GetAllMembers([FromRoute] Guid groupId)
        {
            var result = await _groupRepository.GetMembersAsync(groupId);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            var members = result.Data ?? [];

            var dto = members.Select(m => new GroupMemberReadDto
            {
                MemberId = m.MemberId,
                GroupId = m.GroupId,
                JoinedAt = m.JoinedAt,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email,
            });

            return Ok(dto);
        }

        /// <summary>
        /// Retrieves every group the user has joinded.
        /// </summary>
        /// <param name="userId">Identifier of the user whose joined groups are requested.</param>
        /// <returns>
        /// 200 OK with a collection of <see cref="GroupReadDto"/>; 400 Bad Request when the repository reports an error.
        /// </returns>
        [Route("v1/users/{userId:guid}/groups")]
        [HttpGet]
        public async Task<IActionResult> GetGroupsForUserAsync([FromRoute] Guid userId)
        {
            var groupsList = await _groupRepository.GetGroupsForUserAsync(userId);

            if (!groupsList.IsSuccess)
            {
                return BadRequest(groupsList.ErrorMessage);
            }

            var groups = groupsList.Data ?? [];

            var result = groups.Select(ToGroupReadDto).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Retrieves every group the user owns.
        /// </summary>
        /// <param name="userId">Identifier of the user who owns some groups.</param>
        /// <returns>
        /// 200 OK with a collection of <see cref="GroupReadDto"/>; 400 Bad Request when the repository reports an error.
        /// </returns>
        [Route("v1/users/{userId:guid}/owned")]
        [HttpGet]
        public async Task<IActionResult> GetOwnedGroupsForUserAsync([FromRoute] Guid userId)
        {
            var groupsList = await _groupRepository.GetOwnedGroupsForUserAsync(userId);

            if (!groupsList.IsSuccess)
            {
                return BadRequest(groupsList.ErrorMessage);
            }

            var groups = groupsList.Data ?? [];

            var result = groups.Select(ToGroupReadDto).ToList();

            return Ok(result);
        }

        private Guid GetOIdFromToken()
        {
            var oidClaim = HttpContext.User.GetObjectId();
            return oidClaim != null ? Guid.Parse(oidClaim) : Guid.Empty;
        }

        private GroupReadDto ToGroupReadDto(Group group) =>
            new()
            {
                GroupId = group.GroupId,
                Name = group.Name,
                Description = group.Description,
                OwnerId = group.OwnerId,
                OwnerFirstName = group.Owner != null ? group.Owner.FirstName : string.Empty,
                OwnerLastName = group.Owner != null ? group.Owner.LastName : string.Empty,
                OwnerEmail = group.Owner != null ? group.Owner.Email : string.Empty,
                CreatedAt = group.CreatedAt,
            };
    }
}
