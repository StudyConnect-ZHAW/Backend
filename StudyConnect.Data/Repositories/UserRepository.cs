using System;
using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Common;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;

namespace StudyConnect.Data.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(StudyConnectDbContext context) : base(context)
    {

    }

    public async Task<OperationResult<bool>> AddAsync(User user)
    {
        // Validate the incoming user object
        if (user == null)
        {
            return OperationResult<bool>.Failure("User cannot be null.");
        }

        // Check if the user GUID is valid
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserGuid == user.UserGuid);
        if (existingUser != null)
        {
            return OperationResult<bool>.Failure("A user with the same GUID already exists.");
        }

        // Add the user to the database
        try
        {
            // Get Student role from the database by ID
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.URoleId == Guid.Parse("00000000-0000-0000-0000-000000000001"));
            if (userRole == null)
            {
                return OperationResult<bool>.Failure("User role not found.");
            }

            var userToAdd = new Entities.User
            {
                UserGuid = user.UserGuid,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                URole = userRole
            };

            // Add the user to the database context
            await _context.Users.AddAsync(userToAdd);
            // Save changes to the database
            await _context.SaveChangesAsync();
            
            return OperationResult<bool>.Success(true);
        }
        catch (InvalidOperationException ex)
        {
            return OperationResult<bool>.Failure($"Failed to add user: {ex.Message}");
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"An error occurred while adding the user: {ex.Message}");
        }

    }

    public Task<OperationResult<bool>> DeleteAsync(Guid guid)
    {
        throw new NotImplementedException();
    }

    public async Task<OperationResult<User?>> GetByIdAsync(Guid guid)
    {
        if (guid == Guid.Empty)
        {
            return OperationResult<User?>.Failure("Invalid GUID.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserGuid == guid);
        if (user == null)
        {
            return await Task.FromResult(OperationResult<User?>.Failure("User not found."));
        }

        var userToReturn = new User
        {
            UserGuid = user.UserGuid,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };

        return OperationResult<User?>.Success(userToReturn);
    }

    public async Task<OperationResult<bool>> UpdateAsync(User user)
    {
        if (user == null)
        {
            return OperationResult<bool>.Failure("User cannot be null.");
        }
        if (user.UserGuid == Guid.Empty)
        {
            return OperationResult<bool>.Failure("Invalid GUID.");
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserGuid == user.UserGuid);
        if (existingUser == null)
        {
            return OperationResult<bool>.Success(false);
        }

        try
        {
            // Update the user properties
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;

            await _context.SaveChangesAsync();

            return await Task.FromResult(OperationResult<bool>.Success(true));
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"An error occurred while updating the user: {ex.Message}");
        }

    }

}
