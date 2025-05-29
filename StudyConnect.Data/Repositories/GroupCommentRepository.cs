using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Common;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.Data.Repositories;

public class GroupCommentRepository : BaseRepository, IGroupCommentRepository
{
    public GroupCommentRepository(StudyConnectDbContext context)
        : base(context) { }

    public async Task<OperationResult<GroupComment>> AddAsync(
        Guid userId,
        Guid groupId,
        Guid postId,
        GroupComment comment
    )
    {
        var member = await GetValidMember(userId, groupId);
        if (member == null)
            return OperationResult<GroupComment>.Failure("Member not found.");

        if (!await IsValidPost(postId))
            return OperationResult<GroupComment>.Failure(PostNotFound);

        try
        {
            // Create the comment entity and populate it with the relevant data
            var result = new Entities.GroupComment
            {
                Content = comment.Content,
                GroupPostId = postId,
                GroupMemberId = member.GroupMemberId,
            };

            await _context.AddAsync(result);
            await _context.SaveChangesAsync();

            var created = await _context
                .GroupComments.Include(p => p.GroupMember)
                .ThenInclude(gm => gm.Member)
                .FirstOrDefaultAsync(p => p.GroupPostId == result.GroupPostId);

            if (created is null)
                return OperationResult<GroupComment>.Failure(
                    $"{UnknownError}: Failed to retrieve the newly created post."
                );

            return OperationResult<GroupComment>.Success(MapCommentToModel(created));
        }
        catch (Exception ex)
        {
            return OperationResult<GroupComment>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<IEnumerable<GroupComment>>> GetAllofPostAsync(Guid postId)
    {
        if (!await IsValidPost(postId))
            return OperationResult<IEnumerable<GroupComment>>.Failure(PostNotFound);

        // Retrieve all comments for the specified post, including related entities
        var comments = await _context
            .GroupComments.AsNoTracking()
            .Include(gc => gc.GroupMember)
            .ThenInclude(gm => gm.Member)
            .Where(cm => cm.GroupPostId == postId)
            .ToListAsync();

        // Test list for data
        if (!comments.Any())
            return OperationResult<IEnumerable<GroupComment>>.Success(new List<GroupComment>());

        var result = comments.Select(MapCommentToModel).ToList();

        return OperationResult<IEnumerable<GroupComment>>.Success(result);
    }

    public async Task<OperationResult<GroupComment?>> GetByIdAsync(Guid commentId)
    {
        if (commentId == Guid.Empty)
            return OperationResult<GroupComment?>.Failure(InvalidCommentId);

        // Retrieve a comment including related entities
        var result = await _context
            .GroupComments.AsNoTracking()
            .Include(gm => gm.GroupMember)
            .ThenInclude(gm => gm.Member)
            .FirstOrDefaultAsync(c => c.GroupCommentId == commentId);

        // Test for data
        return result == null
            ? OperationResult<GroupComment?>.Failure(CommentNotFound)
            : OperationResult<GroupComment?>.Success(MapCommentToModel(result));
    }

    public async Task<OperationResult<GroupComment>> UpdateAsync(
        Guid userId,
        Guid groupId,
        Guid commentId,
        GroupComment comment
    )
    {
        if (userId == Guid.Empty)
            return OperationResult<GroupComment>.Failure(InvalidUserId);

        if (groupId == Guid.Empty)
            return OperationResult<GroupComment>.Failure("Invali Group id.");

        if (commentId == Guid.Empty)
            return OperationResult<GroupComment>.Failure(InvalidCommentId);

        // Retrieve the comment and ensure the user is authorized to access it
        var (result, error) = await GetAuthorizedCommentAsync(userId, groupId, commentId);
        if (result == null)
            return OperationResult<GroupComment>.Failure(error ?? UnknownError);

        try
        {
            // Update the comment's content and metadata
            result.Content = comment.Content;
            result.UpdatedAt = DateTime.UtcNow;
            result.IsEdited = true;

            await _context.SaveChangesAsync();

            return OperationResult<GroupComment>.Success(MapCommentToModel(result));
        }
        catch (Exception ex)
        {
            return OperationResult<GroupComment>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid userId, Guid groupId, Guid commentId)
    {
        if (userId == Guid.Empty)
            return OperationResult<bool>.Failure(InvalidUserId);

        if (groupId == Guid.Empty)
            return OperationResult<bool>.Failure("Invalid Group id.");

        if (commentId == Guid.Empty)
            return OperationResult<bool>.Failure(InvalidCommentId);

        // Retrieve the comment and ensure the user is authorized to access it
        var (result, error) = await GetAuthorizedCommentAsync(userId, groupId, commentId);
        if (result == null)
            return OperationResult<bool>.Failure(error ?? UnknownError);

        try
        {
            _context.GroupComments.Remove(result);
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
    /// Validates if a post exists in the database.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns><c>true</c> if the post exists; otherwise, <c>false</c>.</returns>
    private async Task<bool> IsValidPost(Guid postId) =>
        postId != Guid.Empty && await _context.GroupPosts.AnyAsync(p => p.GroupPostId == postId);

    /// <summary>
    /// Tests a comment for existence and authorization.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="postId">The unique identifier of the comment.</param>
    /// <returns>A post entity on succes or an errormessage on failure.</returns>
    private async Task<(
        Entities.GroupComment? Comment,
        string? ErrorMessage
    )> GetAuthorizedCommentAsync(Guid userId, Guid groupId, Guid commentId)
    {
        var comment = await _context
            .GroupComments.Include(cm => cm.GroupMember)
            .ThenInclude(gm => gm.Member)
            .FirstOrDefaultAsync(c => c.GroupCommentId == commentId);

        if (comment == null || comment.IsDeleted)
            return (null, CommentNotFound);

        if (comment.GroupMember.MemberId != userId || comment.GroupMember.GroupId != groupId)
            return (null, NotAuthorized);

        return (comment, null);
    }

    /// <summary>
    /// A helper function to map a forum comment entity to its model representation.
    /// </summary>
    /// <param name="comment">A comment entity to transform.</param>
    /// <returns>A forum comment model object.</returns>
    ///
    private GroupComment MapCommentToModel(Entities.GroupComment comment) =>
        new()
        {
            GroupCommentId = comment.GroupCommentId,
            Content = comment.IsDeleted ? string.Empty : comment.Content,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            IsEdited = comment.IsEdited,
            GroupPostId = comment.GroupPostId,
            groupMember = comment.GroupMember.ToGroupMember(),
        };
}
