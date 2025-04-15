using System;
using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces;

public interface IUserRepository
{   

    Task<OperationResult<User?>> GetByIdAsync(Guid guid);
    Task<OperationResult<bool>> AddAsync(User user);
    Task<OperationResult<bool>> UpdateAsync(User user);
    Task<OperationResult<bool>> DeleteAsync(Guid guid);
}
