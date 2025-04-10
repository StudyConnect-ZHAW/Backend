using StudyConnect.Data.Entities;

namespace StudyConnect.Data.Interfaces;

/// <summary>
/// Interface for accessing and manipulating user data in the database.
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
}
