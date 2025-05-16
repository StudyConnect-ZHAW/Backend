using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces.Services;

public interface ICommentService
{
    Task<OperationResult<ForumComment>> AddCommentAsync(ForumComment comment, Guid userId, Guid postId, Guid? parentId);

    Task<OperationResult<IEnumerable<ForumComment?>>> GetAllCommentsOfPostAsync(Guid postId);

    Task<OperationResult<ForumComment?>> GetCommentByIdAsync(Guid commentId);


    Task<OperationResult<ForumComment>> UpdateCommentAsync(Guid commentId, Guid userId, ForumComment comment);

    Task<OperationResult<bool>> DeleteCommentAsync(Guid commentId, Guid userId);
}
