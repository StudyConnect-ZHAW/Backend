namespace StudyConnect.Data.Interfaces;

using StudyConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Interface for accessing and manipulating roles of group members.
/// </summary>
public interface IMemberRoleRepository
{
    Task<MemberRole?> GetByIdAsync(Guid id);
    Task<IEnumerable<MemberRole>> GetAllAsync();
    Task AddAsync(MemberRole entity);
    Task UpdateAsync(MemberRole entity);
    Task DeleteAsync(MemberRole entity);
}