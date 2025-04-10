namespace StudyConnect.Data.Interfaces;

using StudyConnect.Data.Entities;

/// <summary>
/// Interface for accessing and manipulating general user roles in the database.
/// </summary>
public interface IUserRoleRepository
{
    Task<UserRole> GetByIdAsync(int id);
    Task<IEnumerable<UserRole>> GetAllAsync();
    Task AddAsync(UserRole entity);
    Task UpdateAsync(UserRole entity);
    Task DeleteAsync(UserRole entity);
}