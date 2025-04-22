using System;
using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Common;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;

namespace StudyConnect.Data.Repositories;

public class GroupRepository : BaseRepository, IGroupRepository
{
    public GroupRepository(StudyConnectDbContext context) : base(context)
    {
        
    }
public async Task<OperationResult<Group?>> GetByIdAsync(Guid groupId)
{
    if (groupId == Guid.Empty)
    {
        return OperationResult<Group?>.Failure("Invalid GUID.");
    }

    var entity = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);
    if (entity == null)
    {
        return await Task.FromResult(OperationResult<Group?>.Failure("Group not found."));
    }

    var groupToReturn = new Group
    {
        GroupId = entity.GroupId,
        OwnerId = entity.OwnerId,
        Name = entity.Name,
        Description = entity.Description,
    };

    return OperationResult<Group?>.Success(groupToReturn);
}

    public async Task<OperationResult<bool>> UpdateAsync(Group group)
{
    if (group == null)
    {
        return OperationResult<bool>.Failure("Group cannot be null.");
    }

    if (group.GroupId == Guid.Empty)
    {
        return OperationResult<bool>.Failure("Invalid GUID.");
    }

    var existingGroup = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == group.GroupId);
    if (existingGroup == null)
    {
        return OperationResult<bool>.Failure("Group not found.");
    }

    try
    {
        //Update the group properties
        existingGroup.Name = group.Name;
        existingGroup.Description = group.Description;

        await _context.SaveChangesAsync();

        return await Task.FromResult(OperationResult<bool>.Success(true));
    }
    catch (Exception ex)
    {
        return OperationResult<bool>.Failure($"An error occurred while updating the group: {ex.Message}");
    }
}
public async Task<OperationResult<bool>> DeleteAsync(Guid groupId)
{
    if (groupId == Guid.Empty)
    {
        return OperationResult<bool>.Failure("Invalid GUID.");
    }

    var entity = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);
    if (entity == null)
    {
        return OperationResult<bool>.Failure("Group not found.");
    }

    try
    {
        _context.Groups.Remove(entity);
        await _context.SaveChangesAsync();
        return OperationResult<bool>.Success(true);
    }
    catch (Exception ex)
    {
        return OperationResult<bool>.Failure($"An error occurred while deleting the group: {ex.Message}");
    }
}
public async Task<OperationResult<bool>> AddAsync(Group group)
{
    if (group == null)
    {
        return OperationResult<bool>.Failure("Group cannot be null.");
    }

    var existingGroup = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == group.GroupId);
    if (existingGroup != null)
    {
        return OperationResult<bool>.Failure("A group with the same ID already exists.");
    }

    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserGuid == group.OwnerId);
    if (user == null)
    {
        return OperationResult<bool>.Failure("Owner user not found.");
    }

    try
    {
        var entity = new Data.Entities.Group
        {
            GroupId = group.GroupId != Guid.Empty ? group.GroupId : Guid.NewGuid(),
            OwnerId = group.OwnerId,
            Name = group.Name,
            Description = group.Description,
            Owner = user 
        };

        await _context.Groups.AddAsync(entity);
        await _context.SaveChangesAsync();

        return OperationResult<bool>.Success(true);
    }
    catch (Exception ex)
    {
        return OperationResult<bool>.Failure($"An error occurred while adding the group: {ex.Message}");
    }
}

public async Task<OperationResult<IEnumerable<Group>>> GetAllAsync()
{
    try
    {
        var entities = await _context.Groups.ToListAsync();

        var models = entities.Select(g => new Group
        {
            GroupId = g.GroupId,
            OwnerId = g.OwnerId,
            Name = g.Name,
            Description = g.Description,
        });

        return OperationResult<IEnumerable<Group>>.Success(models);
    }
    catch (Exception ex)
    {
        return OperationResult<IEnumerable<Group>>.Failure($"An error occurred while fetching groups: {ex.Message}");
    }
}


}
