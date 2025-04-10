using StudyConnect.Data.Entities;

namespace StudyConnect.Data.Interfaces;

/// <summary>
/// Interface for user repository operations.
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
}
