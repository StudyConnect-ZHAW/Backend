namespace StudyConnect.Data.Interfaces;

using StudyConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IForumPostRepository
{
    Task<ForumPost?> GetByIdAsync(Guid id);
    Task<IEnumerable<ForumPost>> GetAllAsync();
    Task AddAsync(ForumPost entity);
    Task UpdateAsync(ForumPost entity);
    Task DeleteAsync(ForumPost entity);
}