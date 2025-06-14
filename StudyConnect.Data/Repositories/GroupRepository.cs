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
            return OperationResult<Group?>.Failure(InvalidGroupId);
        }

        var entity = await _context
            .Groups.Include(g => g.Owner)
            .FirstOrDefaultAsync(g => g.GroupId == groupId);

        if (entity == null)
        {
            return OperationResult<Group?>.Success(null);
        }

        return OperationResult<Group?>.Success(entity.ToGroupModel());
    }

    public async Task<OperationResult<Group>> UpdateAsync(Group group)
    {
        if (group.GroupId == Guid.Empty)
            return OperationResult<Group>.Failure(InvalidGroupId);

        if (group.OwnerId == Guid.Empty)
            return OperationResult<Group>.Failure(InvalidUserId);

        var existingGroup = await _context
            .Groups.Include(g => g.Owner)
            .FirstOrDefaultAsync(g => g.GroupId == group.GroupId);

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
            return OperationResult<Group>.Failure(
                $"An error occurred while updating the group: {ex.Message}"
            );
        }
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid userId, Guid groupId)
    {
        if (groupId == Guid.Empty)
            return OperationResult<bool>.Failure(InvalidGroupId);

        if (userId == Guid.Empty)
            return OperationResult<bool>.Failure(InvalidUserId);

        var entity = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);
        if (entity == null)
        {
            return OperationResult<bool>.Failure(GroupNotFound);
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
            return OperationResult<bool>.Failure(
                $"An error occurred while deleting the group: {ex.Message}"
            );
        }
    }

    public async Task<OperationResult<Group>> AddAsync(Group group)
    {
        var existingGroup = await _context.Groups.FirstOrDefaultAsync(g => g.Name == group.Name);
        if (existingGroup != null)
        {
            return OperationResult<Group>.Failure(NameTaken);
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
            await _context.Groups.Entry(entity).Reference(g => g.Owner).LoadAsync();
            return OperationResult<Group>.Success(entity.ToGroupModel());
        }
        catch (Exception ex)
        {
            return OperationResult<Group>.Failure(
                $"An error occurred while fetching groups: {ex.Message}"
            );
        }
    }

    public async Task<OperationResult<IEnumerable<Group>>> GetAllAsync()
    {
        try
        {
            var entities = await _context.Groups.Include(g => g.Owner).ToListAsync();

            var models = entities.Select(g => g.ToGroupModel());

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
                return OperationResult<IEnumerable<GroupMember>>.Failure(InvalidGroupId);
            }
            var groupmembers = await _context
                .GroupMembers.AsNoTracking()
                .Include(gm => gm.Member)
                .Where(gm => gm.GroupId == GroupId)
                .ToListAsync();

            if (groupmembers == null)
            {
                return OperationResult<IEnumerable<GroupMember>>.Failure(GroupNotFound);
            }

            var members = groupmembers.Select(g => g.ToMemberModel());

            return OperationResult<IEnumerable<GroupMember>>.Success(members);
        }
        catch (Exception ex)
        {
            return OperationResult<IEnumerable<GroupMember>>.Failure(
                $"An error occurred while fetching members: {ex.Message}"
            );
        }
    }

    public async Task<OperationResult<IEnumerable<Group>>> GetGroupsForUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return OperationResult<IEnumerable<Group>>.Failure(InvalidUserId);
        }

        var userGroups = await _context
            .Groups.Include(g => g.GroupMembers)
            .Include(g => g.Owner)
            .Where(g => g.GroupMembers.Any(gm => gm.MemberId == userId))
            .ToListAsync();

        if (userGroups == null)
        {
            return OperationResult<IEnumerable<Group>>.Success(new List<Group>());
        }

        var groups = userGroups.Select(g => g.ToGroupModel()).ToList();

        return OperationResult<IEnumerable<Group>>.Success(groups);
    }

    public async Task<OperationResult<IEnumerable<Group>>> GetOwnedGroupsForUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return OperationResult<IEnumerable<Group>>.Failure(InvalidUserId);
        }

        var existingGroups = await _context
            .Groups.Include(g => g.Owner)
            .Where(g => g.OwnerId == userId)
            .ToListAsync();

        if (existingGroups == null)
        {
            return OperationResult<IEnumerable<Group>>.Success(new List<Group>());
        }

        var groups = existingGroups.Select(g => g.ToGroupModel()).ToList();

        return OperationResult<IEnumerable<Group>>.Success(groups);
    }
}
