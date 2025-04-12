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
    Task<IEnumerable<User>> GetByName(string name);
    Task<IEnumerable<User>> GetBySurname(string surname);
    Task<IEnumerable<User>> GetByNameAndSurname(string name, string surname);
}
