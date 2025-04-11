namespace StudyConnect.Data.Interfaces;

using StudyConnect.Data.Entities;


public interface IUserRoleRepository
{
    Task<UserRole?> GetByIdAsync(Guid guid);
    Task<IEnumerable<UserRole>> GetAllAsync();
    Task AddAsync(UserRole entity);
    Task UpdateAsync(UserRole entity);
    Task DeleteAsync(Guid guid);
    Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId);
}