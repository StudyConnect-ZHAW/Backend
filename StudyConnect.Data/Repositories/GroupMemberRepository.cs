using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Common;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.Data.Repositories;

public class GroupMemberRepository : BaseRepository, IGroupMemberRepository
{
    public GroupMemberRepository(StudyConnectDbContext context)
        : base(context) { }

    public async Task<OperationResult<GroupMember>> AddMemberAsync(Guid UserId, Guid GroupId)
    {
        if (!await IsValidUser(UserId))
            return OperationResult<GroupMember>.Failure(UserNotFound);

        if (!await IsValidGroup(GroupId))
            return OperationResult<GroupMember>.Failure(GroupNotFound);

        if (await IsValidMember(UserId, GroupId))
            return OperationResult<GroupMember>.Failure(MemberTaken);

        var member = new Entities.GroupMember
        {
            MemberId = UserId,
            GroupId = GroupId,
            MemberRoleId = Guid.Parse("00000000-0000-0000-0000-000000000010"),
        };

        try
        {
            await _context.GroupMembers.AddAsync(member);
            await _context.SaveChangesAsync();
            await _context.GroupMembers.Entry(member).Reference(m => m.Member).LoadAsync();
        }
        catch (Exception ex)
        {
            return OperationResult<GroupMember>.Failure($"Failed to add member: {ex.Message}");
        }

        return OperationResult<GroupMember>.Success(member.ToMemberModel());
    }

    public async Task<OperationResult<bool>> DeleteMemberAsync(Guid UserId, Guid GroupId)
    {
        if (UserId == Guid.Empty)
            return OperationResult<bool>.Failure(InvalidUserId);
        if (GroupId == Guid.Empty)
            return OperationResult<bool>.Failure(InvalidGroupId);

        var entity = await _context.GroupMembers.FirstOrDefaultAsync(g =>
            g.GroupId == GroupId && g.MemberId == UserId
        );
        if (entity == null)
        {
            return OperationResult<bool>.Failure(MemberNotFound);
        }

        try
        {
            _context.GroupMembers.Remove(entity);
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"Failed to delete member: {ex.Message}");
        }
    }

    private async Task<bool> IsValidGroup(Guid GroupId) =>
        GroupId != Guid.Empty && await _context.Groups.AnyAsync(u => u.GroupId == GroupId);

    private async Task<bool> IsValidUser(Guid userId) =>
        userId != Guid.Empty && await _context.Users.AnyAsync(u => u.UserGuid == userId);

    private async Task<bool> IsValidMember(Guid userId, Guid groupId) =>
        await _context.GroupMembers.AnyAsync(m => m.MemberId == userId && m.GroupId == groupId);
}
