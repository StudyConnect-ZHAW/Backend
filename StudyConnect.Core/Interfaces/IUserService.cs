using StudyConnect.Core.Entities;
using StudyConnect.Core.Common;

namespace StudyConnect.Core.Interfaces;

/// <summary>
/// Interface for user service operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The user, or null if not found.</returns>
    Task<User?> GetUserAsync(Guid id);
    
    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>A collection of all users.</returns>
    Task<IEnumerable<User>> GetAllUsersAsync();
    
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="firstName">The user's first name.</param>
    /// <param name="lastName">The user's last name.</param>
    /// <param name="email">The user's email.</param>
    /// <param name="roleId">Optional role ID. If not provided, the Student role will be used.</param>
    /// <returns>A result containing the created user if successful.</returns>
    Task<Result<User>> CreateUserAsync(string firstName, string lastName, string email, Guid? roleId = null);
    
    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="firstName">The user's first name.</param>
    /// <param name="lastName">The user's last name.</param>
    /// <param name="email">The user's email.</param>
    /// <param name="roleId">Optional role ID to update.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result> UpdateUserAsync(Guid id, string firstName, string lastName, string email, Guid? roleId = null);
    
    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result> DeleteUserAsync(Guid id);
    
    /// <summary>
    /// Verifies a Microsoft token and creates or updates a user.
    /// </summary>
    /// <param name="userGuid">The user's GUID from Microsoft identity.</param>
    /// <param name="token">The Microsoft authentication token.</param>
    /// <returns>A result containing the user if successful.</returns>
    Task<Result<User>> VerifyAndCreateUserAsync(Guid userGuid, string token);
    
    /// <summary>
    /// Gets the Student role ID.
    /// </summary>
    /// <returns>The Student role ID.</returns>
    Task<Guid> GetStudentRoleIdAsync();
}
