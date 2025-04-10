namespace StudyConnect.Data.Interfaces;

using StudyConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Interface for accessing and manipulating forum categories.
/// </summary>
public interface IForumCategoryRepository
{
    Task<ForumCategory?> GetByIdAsync(Guid id);
    Task<IEnumerable<ForumCategory>> GetAllAsync();
    Task AddAsync(ForumCategory entity);
    Task UpdateAsync(ForumCategory entity);
    Task DeleteAsync(ForumCategory entity);
}