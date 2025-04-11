namespace StudyConnect.Data.Interfaces;

using StudyConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


public interface IForumCategoryRepository
{
    Task<ForumCategory?> GetByIdAsync(Guid guid);
    Task<IEnumerable<ForumCategory>> GetAllAsync();
    Task AddAsync(ForumCategory entity);
    Task UpdateAsync(ForumCategory entity);
    Task DeleteAsync(Guid guid);
}