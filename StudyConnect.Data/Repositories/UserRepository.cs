using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Entities;
using StudyConnect.Core.Interfaces;

namespace StudyConnect.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly StudyConnectDbContext _dbContext;

    public UserRepository(StudyConnectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var userToDelete = await _dbContext.Users.FindAsync(id);
        if (userToDelete != null)
        {
            _dbContext.Users.Remove(userToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}
