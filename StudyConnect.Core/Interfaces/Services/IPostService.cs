using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

namespace StudyConnect.Core.Interfaces.Services;

public interface IPostService
{
    Task<OperationResult<ForumPost>> AddPostAsync(Guid userId, Guid categoryId, ForumPost post);

    Task<OperationResult<IEnumerable<ForumPost>>> SearchPostAsync(
          Guid? userId,
          string? categoryName,
          string? title,
          DateTime? fromDate,
          DateTime? toDate
    );

    Task<OperationResult<ForumPost>> GetPostByIdAsync(Guid postId);

    Task<OperationResult<ForumPost>> UpdatePostAsync(Guid userId, Guid postId, ForumPost post);

    Task<OperationResult<bool>> DeletePostAsync(Guid userId, Guid postId);
}
