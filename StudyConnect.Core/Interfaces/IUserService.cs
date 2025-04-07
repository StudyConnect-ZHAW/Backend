using StudyConnect.Core.Entities;
using StudyConnect.Core.Common;

namespace StudyConnect.Core.Interfaces;

public interface IUserService
{
    Task<User?> GetUserAsync(Guid id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<Result<User>> CreateUserAsync(string firstName, string lastName, string email);
    Task<Result> UpdateUserAsync(Guid id, string firstName, string lastName, string email);
    Task<Result> DeleteUserAsync(Guid id);
}