using System;
using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces;

public interface IGroupRepository
{
        Task<OperationResult<Group?>> GetByIdAsync(Guid groupId);
        Task<OperationResult<IEnumerable<Group>>> GetAllAsync();
        Task<OperationResult<bool>> AddAsync(Group group);
        Task<OperationResult<bool>> UpdateAsync(Group group);
        Task<OperationResult<bool>> DeleteAsync(Guid groupId);
}
