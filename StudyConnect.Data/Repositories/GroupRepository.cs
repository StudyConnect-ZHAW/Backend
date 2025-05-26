using System;
using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Common;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using static StudyConnect.Core.Common.ErrorMessages;

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

    public async Task<OperationResult<Group>> UpdateAsync(Group group)
    {
        if (group.GroupId == Guid.Empty || group.OwnerId == Guid.Empty)
        {
            return OperationResult<Group>.Failure("Invalid GUID.");
        }

        var existingGroup = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == group.GroupId);
        if (existingGroup == null)
        {
            return OperationResult<Group>.Failure(GroupNotFound);
        }

        if (existingGroup.OwnerId != group.OwnerId)
            return OperationResult<Group>.Failure(NotAuthorized);

        try
        {
            //Update the group properties
            existingGroup.Name = group.Name;
            existingGroup.Description = group.Description;

            await _context.SaveChangesAsync();

            return OperationResult<Group>.Success(existingGroup.ToGroupModel());
        }
        catch (Exception ex)
        {
            return OperationResult<Group>.Failure($"An error occurred while updating the group: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid userId, Guid groupId)
    {
        if (groupId == Guid.Empty || userId == Guid.Empty)
        {
            return OperationResult<bool>.Failure("Invalid GUID.");
        }

        var entity = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);
        if (entity == null)
        {
            return OperationResult<bool>.Success(false);
        }

        if (entity.OwnerId != userId)
            return OperationResult<bool>.Failure(NotAuthorized);

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
        var existingGroup = await _context.Groups.FirstOrDefaultAsync(g => g.Name == group.Name);
        if (existingGroup != null)
        {
            return OperationResult<Group>.Failure("A group with the same name already exists.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserGuid == group.OwnerId);
        if (user == null)
        {
            return OperationResult<Group>.Failure(UserNotFound);
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
            var groupmembers = await _context.GroupMembers
                .AsNoTracking()
                .Include(gm => gm.Member)
                .Where(gm => gm.GroupId == GroupId)
                .ToListAsync();

            if (groupmembers == null)
            {
                return OperationResult<IEnumerable<GroupMember>>.Failure("Group not found");
            }

            var members = groupmembers.Select(g => new GroupMember
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

    public async Task<OperationResult<IEnumerable<Group>>> GetGroupsForUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return OperationResult<IEnumerable<Group>>.Failure("Invalid GUID.");
        }

        var existingMembers = await _context.GroupMembers
        .Include(gm => gm.Group)
        .Where(u => u.MemberId == userId)
        .ToListAsync();

        if (existingMembers == null)
        {
            return OperationResult<IEnumerable<Group>>.Success([]);
        }

        var groups = existingMembers.Select(gm => new Group
        {
            GroupId = gm.GroupId,
            OwnerId = gm.Group.OwnerId,
            Name = gm.Group.Name,
            Description = gm.Group.Description,
            CreatedAt = gm.Group.CreatedAt
        }).ToList();

        return OperationResult<IEnumerable<Group>>.Success(groups);
    }

    public async Task<OperationResult<IEnumerable<Group>>> GetOwnedGroupsForUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return OperationResult<IEnumerable<Group>>.Failure("Invalid GUID.");
        }

        var existingGroups = await _context.Groups
        .Include(g => g.Owner)
        .Where(g => g.OwnerId == userId)
        .ToListAsync();

        if (existingGroups == null)
        {
            return OperationResult<IEnumerable<Group>>.Success([]);
        }

        var groups = existingGroups.Select(gm => new Group
        {
            GroupId = gm.GroupId,
            OwnerId = gm.OwnerId,
            Name = gm.Name,
            Description = gm.Description,
            CreatedAt = gm.CreatedAt
        }).ToList();

        return OperationResult<IEnumerable<Group>>.Success(groups);
    }
}
