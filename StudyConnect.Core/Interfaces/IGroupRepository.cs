using System;
using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces;

/// <summary>
/// Retrieves a group by its unique identifier.
/// </summary>
/// <param name="groupId">The ID of the group to retrieve.</param>
/// <returns>An operation result containing the group if found, or null.</returns>
public interface IGroupRepository
{
    /// <summary>
    /// Retrieves  group by id from the system.
    /// </summary>
    /// <returns>An operation result containing a collection of all groups.</returns>
    Task<OperationResult<Group?>> GetByIdAsync(Guid groupId);

    /// <summary>
    /// Retrieves all groups from the system.
    /// </summary>
    /// <returns>An operation result containing a collection of all groups.</returns>
    Task<OperationResult<IEnumerable<Group>>> GetAllAsync();

    /// <summary>
    /// Retrieves all groupsmember from the group.
    /// </summary>
    /// <returns>An operation result containing a collection of all groupsmembers.</returns>
    Task<OperationResult<IEnumerable<GroupMember>>> GetMembersAsync(Guid GroupId);

    /// <summary>
    /// Retrieves all groups the specified user is a member of.
    /// </summary>
    /// <param name="userId">The user’s unique identifier.</param>
    /// <returns>
    // An <see cref="OperationResult{T}"/> whose <c>Data</c> property
    /// contains a collection of <see cref="Group"/> objects.
    /// </returns>
    Task<OperationResult<IEnumerable<Group>>> GetGroupsForUserAsync(Guid userId);

    /// <summary>
    /// Retrieves all groups the specified user is a owner of.
    /// </summary>
    /// <param name="userId">The user’s unique identifier.</param>
    /// <returns>
    // An <see cref="OperationResult{T}"/> whose <c>Data</c> property
    /// contains a collection of <see cref="Group"/> objects.
    /// </returns>
    Task<OperationResult<IEnumerable<Group>>> GetOwnedGroupsForUserAsync(Guid userId);


    /// <summary>
    /// Adds a new group to the system.
    /// </summary>
    /// <param name="group">The group to add.</param>
    /// <returns>An operation result indicating success or failure.</returns>
    Task<OperationResult<Group>> AddAsync(Group group);

    /// <summary>
    /// Updates an existing group in the system.
    /// </summary>
    /// <param name="group">The group with updated data.</param>
    /// <returns>An operation result indicating success or failure.</returns>
    Task<OperationResult<bool>> UpdateAsync(Group group);

    /// <summary>
    /// Deletes a group by its unique identifier.
    /// </summary>
    /// <param name="groupId">The ID of the group to delete.</param>
    /// <returns>An operation result indicating success or failure.</returns>
    Task<OperationResult<bool>> DeleteAsync(Guid groupId);
}
