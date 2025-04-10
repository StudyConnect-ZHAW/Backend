using Microsoft.EntityFrameworkCore;
using StudyConnect.Data.Entities;
using StudyConnect.Data.Interfaces;

namespace StudyConnect.Data.Repositories;

/// <summary>
/// Repository for accessing and manipulating group data in the database.
/// </summary>
public class GroupRepository : IGroupRepository
{
    private readonly StudyConnectDbContext _context;

    public GroupRepository(StudyConnectDbContext context)
    {
        _context = context;
    }

    public async Task<Group?> GetByIdAsync(Guid id)
    {
        return await _context.Groups.FindAsync(id);
    }

    public async Task<IEnumerable<Group>> GetAllAsync()
    {
        return await _context.Groups.ToListAsync();
    }

    public async Task AddAsync(Group entity)
    {
        await _context.Groups.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Group entity)
    {
        _context.Groups.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Group entity)
    {
        _context.Groups.Remove(entity);
        await _context.SaveChangesAsync();
    }
}