namespace StudyConnect.Data.Interfaces;

using StudyConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGroupMembersRepository
{
    Task<GroupMembers?> GetByIdAsync(Guid id);
    Task<IEnumerable<GroupMembers>> GetAllAsync();
    Task AddAsync(GroupMembers entity);
    Task UpdateAsync(GroupMembers entity);
    Task DeleteAsync(GroupMembers entity);
}