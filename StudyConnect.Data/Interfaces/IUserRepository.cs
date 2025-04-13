using StudyConnect.Data.Entities;

namespace StudyConnect.Data.Interfaces;


public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid guid);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid guid);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetByNameAsync(string name);
    Task<IEnumerable<User>> GetBySurnameAsync(string surname);
    Task<IEnumerable<User>> GetByNameAndSurnameAsync(string name, string surname);
}
