namespace StudyConnect.Data.Interfaces;

using StudyConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


public interface IGroupRepository
{
    Task<Group?> GetByIdAsync(Guid guid);
    Task<IEnumerable<Group>> GetAllAsync();
    Task AddAsync(Group entity);
    Task UpdateAsync(Group entity);
    Task DeleteAsync(Guid guid);
    Task<IEnumerable<Group>> GetByOwnerIdAsync(Guid ownerId);
    Task<IEnumerable<Group>> GetByMemberIdAsync(Guid memberId);
}