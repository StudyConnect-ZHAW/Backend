using System;
using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces;
/// <summary>
/// Contract for adding or removing users from a group.
/// </summary>
public interface IGroupMemberRepository
{
    /// <summary>
    /// Adds the specified user to the specified group.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to add.</param>
    /// <param name="groupId">The unique identifier of the target group.</param>
    /// <returns>
    /// An <see cref="OperationResult{T}"/> whose <c>Data</c> flag is
    /// <c>true</c> when the membership has been created and
    /// <c>false</c> when the membership already exists.
    /// </returns>
    Task<OperationResult<GroupMember>> AddMemberAsync(Guid UserId, Guid GroupId);

    /// <summary>
    /// Removes the specified user from the specified group.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to remove.</param>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns>
    /// An <see cref="OperationResult{T}"/> whose <c>Data</c> flag is
    /// <c>true</c> when the membership has been deleted and
    /// <c>false</c> when no such membership was found.
    /// </returns>
    Task<OperationResult<bool>> DeleteMemberAsync(Guid UserId, Guid GroupId);
}
