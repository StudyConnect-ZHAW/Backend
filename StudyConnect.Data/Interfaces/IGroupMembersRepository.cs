namespace StudyConnect.Data.Interfaces;

using StudyConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


public interface IGroupMembersRepository
{
    Task<GroupMembers?> GetByIdAsync(Guid guid);
    Task<IEnumerable<GroupMembers>> GetAllAsync();
    Task AddAsync(GroupMembers entity);
    Task UpdateAsync(GroupMembers entity);
    Task DeleteAsync(Guid guid);
    Task<IEnumerable<GroupMembers>> GetByGroupIdAsync(Guid groupId);
    Task<IEnumerable<GroupMembers>> GetByMemberIdAsync(Guid memberId);
    Task<IEnumerable<GroupMembers>> GetByRoleIdAsync(Guid roleId);
    Task<IEnumerable<GroupMembers>> GetByGroupIdAndRoleIdAsync(Guid groupId, Guid roleId);
}