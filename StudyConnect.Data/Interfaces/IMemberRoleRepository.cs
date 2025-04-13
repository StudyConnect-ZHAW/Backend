namespace StudyConnect.Data.Interfaces;

using StudyConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


public interface IMemberRoleRepository
{
    Task<MemberRole?> GetByIdAsync(Guid guid);
    Task<IEnumerable<MemberRole>> GetAllAsync();
    Task AddAsync(MemberRole entity);
    Task UpdateAsync(MemberRole entity);
    Task DeleteAsync(Guid guid);
}