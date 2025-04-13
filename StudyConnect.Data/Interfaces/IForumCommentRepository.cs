namespace StudyConnect.Data.Interfaces;

using StudyConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


public interface IForumCommentRepository
{
    Task<ForumComment?> GetByIdAsync(Guid guid);
    Task<IEnumerable<ForumComment>> GetAllAsync();
    Task AddAsync(ForumComment entity);
    Task UpdateAsync(ForumComment entity);
    Task DeleteAsync(Guid guid);
}