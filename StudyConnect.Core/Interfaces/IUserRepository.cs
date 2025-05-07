using System;
using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces;

/// <summary>
/// Interface for managing user-related operations in the system.
/// </summary>
public interface IUserRepository
{   
    /// <summary>
    /// Get a user by their GUID.
    /// </summary>
    /// <param name="guid">The unique identifier of the user.</param>
    /// <returns>An <see cref="OperationResult{T}"/> containing the user if found, or an error message if not.</returns>
    Task<OperationResult<User?>> GetByIdAsync(Guid guid);

    /// <summary>
    /// Add a new user to the system.
    /// </summary>
    /// <param name="user">The user to be added.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<bool>> AddAsync(User user);

    /// <summary>
    /// Update an existing user in the system.
    /// </summary>
    /// <param name="user">The user with updated information.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<bool?>> UpdateAsync(User user);

    /// <summary>
    /// Delete a user from the system.
    /// </summary>
    /// <param name="guid">The unique identifier of the user to be deleted.</param>
    /// <returns>An <see cref="OperationResult{T}"/> indicating success or failure.</returns>
    Task<OperationResult<bool>> DeleteAsync(Guid guid);
}
