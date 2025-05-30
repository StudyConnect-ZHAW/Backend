using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Common;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.Data.Repositories;

public class GroupPostRepository : BaseRepository, IGroupPostRepository
{
    public GroupPostRepository(StudyConnectDbContext context)
        : base(context) { }

    public async Task<OperationResult<GroupPost>> AddAsync(
        Guid userId,
        Guid groupId,
        GroupPost? post
    )
    {
        var member = await GetValidMember(userId, groupId);
        if (member == null)
            return OperationResult<GroupPost>.Failure(MemberNotFound);

        if (post == null)
            return OperationResult<GroupPost>.Failure(PostContentEmpty);

        var newPost = new Entities.GroupPost
        {
            Title = post.Title,
            Content = post.Content,
            GroupMemberId = member.GroupMemberId,
        };

        try
        {
            await _context.GroupPosts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            var created = await _context
                .GroupPosts.Include(p => p.GroupMember)
                .ThenInclude(gm => gm.Member)
                .FirstOrDefaultAsync(p => p.GroupPostId == newPost.GroupPostId);

            if (created is null)
                return OperationResult<GroupPost>.Failure(FailedRetrieve);

            return OperationResult<GroupPost>.Success(MapToPostGroupModel(created));
        }
        catch (Exception ex)
        {
            return OperationResult<GroupPost>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<IEnumerable<GroupPost>>> GetAllAsync(Guid groupId)
    {
        var postsQuery = _context
            .GroupPosts.AsNoTracking()
            .Where(p => p.GroupMember.GroupId == groupId)
            .Include(p => p.GroupComments)
            .Include(p => p.GroupMember)
            .ThenInclude(gm => gm.Member);

        var posts = await postsQuery.ToListAsync();
        var result = posts.Select(MapToPostGroupModel);
        return OperationResult<IEnumerable<GroupPost>>.Success(result);
    }

    public async Task<OperationResult<GroupPost?>> GetByIdAsync(Guid postId)
    {
        if (postId == Guid.Empty)
            return OperationResult<GroupPost?>.Failure(InvalidPostId);

        var post = await _context
            .GroupPosts.AsNoTracking()
            .Include(p => p.GroupComments)
            .Include(p => p.GroupMember)
            .ThenInclude(gm => gm.Member)
            .FirstOrDefaultAsync(p => p.GroupPostId == postId);

        return post == null
            ? OperationResult<GroupPost?>.Failure(PostNotFound)
            : OperationResult<GroupPost?>.Success(MapToPostGroupModel(post));
    }

    public async Task<OperationResult<GroupPost>> UpdateAsync(
        Guid userId,
        Guid groupId,
        Guid postId,
        GroupPost post
    )
    {
        if (userId == Guid.Empty)
            return OperationResult<GroupPost>.Failure(InvalidUserId);

        if (groupId == Guid.Empty)
            return OperationResult<GroupPost>.Failure(InvalidGroupId);

        if (postId == Guid.Empty)
            return OperationResult<GroupPost>.Failure(InvalidPostId);

        var (result, error) = await GetAuthorizedPostAsync(userId, groupId, postId);
        if (result == null)
            return OperationResult<GroupPost>.Failure(error ?? NotAuthorized);

        result.Title = post.Title;
        result.Content = post.Content;
        result.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
            return OperationResult<GroupPost>.Success(MapToPostGroupModel(result));
        }
        catch (Exception ex)
        {
            return OperationResult<GroupPost>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid userId, Guid groupId, Guid postId)
    {
        if (userId == Guid.Empty)
            return OperationResult<bool>.Failure(InvalidUserId);

        if (groupId == Guid.Empty)
            return OperationResult<bool>.Failure(InvalidGroupId);

        if (postId == Guid.Empty)
            return OperationResult<bool>.Failure(InvalidPostId);

        var (result, error) = await GetAuthorizedPostAsync(userId, groupId, postId);
        if (result == null)
            return OperationResult<bool>.Failure(error ?? NotAuthorized);

        try
        {
            _context.GroupPosts.Remove(result);
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    /// <summary>
    /// Validates if a member exists in the database.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns><c>true</c> if the member exists; otherwise, <c>false</c>.</returns>
    private async Task<Entities.GroupMember?> GetValidMember(Guid userId, Guid groupId) =>
        await _context.GroupMembers.FirstOrDefaultAsync(m =>
            m.MemberId == userId && m.GroupId == groupId
        );

    /// <summary>
    /// Tests a post for existence and authorization.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns>A post entity on succes or an errormessage on failure.</returns>
    private async Task<(Entities.GroupPost? Post, string? ErrorMessage)> GetAuthorizedPostAsync(
        Guid userId,
        Guid groupId,
        Guid postId
    )
    {
        var post = await _context
            .GroupPosts.Include(p => p.GroupMember)
            .ThenInclude(gm => gm.Member)
            .FirstOrDefaultAsync(c => c.GroupPostId == postId);

        if (post == null)
            return (null, PostNotFound);

        if (post.GroupMember.MemberId != userId || post.GroupMember.GroupId != groupId)
            return (null, NotAuthorized);

        return (post, null);
    }

    /// <summary>
    /// Maps the post entity to a model.
    /// </summary>
    /// <param name="post">The post entity to map.</param>
    /// <returns>A <see cref="ForumPost"/> </returns>
    private GroupPost MapToPostGroupModel(Entities.GroupPost post) =>
        new()
        {
            GroupPostId = post.GroupPostId,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt.ToUniversalTime(),
            UpdatedAt = post.UpdatedAt.ToUniversalTime(),
            CommentCount = post.GroupComments.Count,
            GroupMember = post.GroupMember!.ToMemberModel(),
        };
}
