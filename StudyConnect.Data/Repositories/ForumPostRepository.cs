using Microsoft.EntityFrameworkCore;
using StudyConnect.Data.Entities;
using StudyConnect.Data.Interfaces;

namespace StudyConnect.Data.Repositories;

/// <summary>
/// Repository for accessing and manipulating forum post data in the database.
/// </summary>
public class ForumPostRepository : IForumPostRepository
{
    private readonly StudyConnectDbContext _context;

    public ForumPostRepository(StudyConnectDbContext context)
    {
        _context = context;
    }

    public async Task<ForumPost?> GetByIdAsync(Guid id)
    {
        return await _context.ForumPosts.FindAsync(id);
    }

    public async Task<IEnumerable<ForumPost>> GetAllAsync()
    {
        return await _context.ForumPosts.ToListAsync();
    }

    public async Task AddAsync(ForumPost entity)
    {
        await _context.ForumPosts.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ForumPost entity)
    {
        _context.ForumPosts.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ForumPost entity)
    {
        _context.ForumPosts.Remove(entity);
        await _context.SaveChangesAsync();
    }
}