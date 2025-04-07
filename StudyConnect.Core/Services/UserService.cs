using StudyConnect.Core.Entities;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Common;

namespace StudyConnect.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<Result<User>> CreateUserAsync(string firstName, string lastName, string email)
    {
        var newUser = new User { UserGuid = Guid.NewGuid(), FirstName = firstName, LastName = lastName, Email = email };
        await _userRepository.AddAsync(newUser);
        return new Result<User> { IsSuccess = true, Value = newUser };
    }

    public async Task<Result> UpdateUserAsync(Guid id, string firstName, string lastName, string email)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            return new Result { IsSuccess = false, Error = "User not found." };
        }

        existingUser.FirstName = firstName;
        existingUser.LastName = lastName;
        existingUser.Email = email;
        await _userRepository.UpdateAsync(existingUser);
        return new Result { IsSuccess = true };
    }

    public async Task<Result> DeleteUserAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id);
        return new Result { IsSuccess = true };
    }
}
