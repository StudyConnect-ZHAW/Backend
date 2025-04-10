namespace StudyConnect.Data.Interfaces;

using StudyConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Interface for accessing and manipulating the groups.
/// </summary>
public interface IGroupRepository
{
    Task<Group?> GetByIdAsync(Guid id);
    Task<IEnumerable<Group>> GetAllAsync();
    Task AddAsync(Group entity);
    Task UpdateAsync(Group entity);
    Task DeleteAsync(Group entity);
}