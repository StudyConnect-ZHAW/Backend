using System;
using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Common;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;

namespace StudyConnect.Data.Repositories;

public class GroupRepository : BaseRepository, IGroupRepository
{
    public GroupRepository(StudyConnectDbContext context)
        : base(context) { }

    public async Task<OperationResult<Group?>> GetByIdAsync(Guid groupId)
    {
        if (groupId == Guid.Empty)
        {
            return OperationResult<Group?>.Failure("Invalid GUID.");
        }

        var entity = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);
        if (entity == null)
        {
            return OperationResult<Group?>.Success(null);
        }

        var groupToReturn = new Group
        {
            GroupId = entity.GroupId,
            OwnerId = entity.OwnerId,
            Name = entity.Name,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt
        };

        return OperationResult<Group?>.Success(groupToReturn);
    }

    public async Task<OperationResult<bool>> UpdateAsync(Group group)
    {
        if (group.GroupId == Guid.Empty)
        {
            return OperationResult<bool>.Failure("Invalid GUID.");
        }
    
        var existingGroup = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == group.GroupId);
        if (existingGroup == null)
        {
            return OperationResult<bool>.Success(false);
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
            return OperationResult<bool>.Success(false);
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

    public async Task<OperationResult<Group>> AddAsync(Group group)
    {
        var existingGroup = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == group.GroupId);
        if (existingGroup != null)
        {
            return OperationResult<Group>.Failure("A group with the same ID already exists.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserGuid == group.OwnerId);
        if (user == null)
        {
            return OperationResult<Group>.Failure("Owner user not found.");
        }

        var entity = new Data.Entities.Group
        {
            OwnerId = group.OwnerId,
            Name = group.Name,
            Description = group.Description,
            Owner = user,
        };

        try
        {
            await _context.Groups.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return OperationResult<Group>.Failure(
                $"An error occurred while fetching groups: {ex.Message}"
            );
        }
        

        // In Domain-/DTO-Modell zurückmappen
            var model = new Group
        {
            GroupId = entity.GroupId,
            OwnerId = entity.OwnerId,
            Name = entity.Name,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt
        };
        return OperationResult<Group>.Success(model);
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
                CreatedAt = g.CreatedAt
            });

            return OperationResult<IEnumerable<Group>>.Success(models);
        }
        catch (Exception ex)
        {
            return OperationResult<IEnumerable<Group>>.Failure(
                $"An error occurred while fetching groups: {ex.Message}"
            );
        }
    }

    public async Task<OperationResult<IEnumerable<GroupMember>>> GetMembersAsync(Guid GroupId)
    {
        try
        {
            if (GroupId == Guid.Empty)
            {
                return OperationResult<IEnumerable<GroupMember>>.Failure("Invalid GroupId");
            }
            var group = await _context.Groups
                .AsNoTracking()
                .Include(g => g.GroupMembers)
                    .ThenInclude(gm => gm.Member)
                .FirstOrDefaultAsync(g => g.GroupId == GroupId);

            if (group == null)
            {
                return OperationResult<IEnumerable<GroupMember>>.Failure("Group not found");
            }

            var members = group.GroupMembers.Select(g => new GroupMember
            {
                GroupId = g.GroupId,
                MemberId = g.MemberId,
                JoinedAt = g.JoinedAt,
                FirstName = g.Member.FirstName,
                LastName = g.Member.LastName,
                Email = g.Member.Email
            });

            return OperationResult<IEnumerable<GroupMember>>.Success(members);

        }
        catch (Exception ex)
        {
            return OperationResult<IEnumerable<GroupMember>>.Failure($"An error occurred while fetching members: {ex.Message}");
        }
    }

}
