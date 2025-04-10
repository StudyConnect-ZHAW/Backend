namespace StudyConnect.Data.Interfaces;

using StudyConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Interface for accessing and manipulating comments under posts or in comment chains.
/// </summary>
public interface IForumCommentRepository
{
    Task<ForumComment?> GetByIdAsync(Guid id);
    Task<IEnumerable<ForumComment>> GetAllAsync();
    Task AddAsync(ForumComment entity);
    Task UpdateAsync(ForumComment entity);
    Task DeleteAsync(ForumComment entity);
}