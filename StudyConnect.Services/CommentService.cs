using StudyConnect.Core.Interfaces.Services;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Common;
using StudyConnect.Core.Models;
using static StudyConnect.Core.Common.ErrorMessages;

public class CommentService : ICommentService
{
    protected readonly ICommentRepository _commmentRepository;
    protected readonly IPostRepository _postRepository;
    protected readonly IUserRepository _userRepository;

    public CommentService(
          ICommentRepository commentRepository,
          IPostRepository postRepository,
          IUserRepository userRepository)
    {
        _commmentRepository = commentRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task<OperationResult<ForumComment>> AddCommentAsync(ForumComment comment, Guid userId, Guid postId, Guid? parentId)
    {
        if (IsInvalid(userId))
            return OperationResult<ForumComment>.Failure(UserNotFound);

        if (IsInvalid(postId))
            return OperationResult<ForumComment>.Failure(PostNotFound);

        if (comment == null)
            return OperationResult<ForumComment>.Failure(CommentContentEmpty);

        var postExists = await _postRepository.PostExistsAsync(postId);
        if (!postExists)
            return OperationResult<ForumComment>.Failure(PostNotFound);

        var userExists = await _userRepository.UserExistsAsync(userId);
        if (!userExists)
            return OperationResult<ForumComment>.Failure(UserNotFound);

        if (parentId != null)
        {
            var isChild = await _commmentRepository.isChildOfPostAsync(postId, (Guid)parentId);
            if (!isChild)
                return OperationResult<ForumComment>.Failure(ParentCommentNotFound);
        }

        try
        {
            var newCommentId = await _commmentRepository.AddAsync(comment, userId, postId, parentId);
            await _postRepository.IncrementCommentCountAsync(postId);
            if (parentId != null)
                await _commmentRepository.IncrementReplyCountAsync((Guid)parentId);

            var result = await _commmentRepository.GetByIdAsync(newCommentId);
            return OperationResult<ForumComment>.Success(result!);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ FULL EXCEPTION:");
            Console.WriteLine(ex.ToString());  // This gives you stack trace + inner exception
            return OperationResult<ForumComment>.Failure($"{UnknownError}: {ex}");
        }
    }

    public async Task<OperationResult<ForumComment?>> GetCommentByIdAsync(Guid commentId)
    {
        if (IsInvalid(commentId))
            return OperationResult<ForumComment?>.Failure(InvalidCommentId);

        var comment = await _commmentRepository.GetByIdAsync(commentId);
        if (comment == null)
            return OperationResult<ForumComment?>.Failure(CommentNotFound);

        return OperationResult<ForumComment?>.Success(comment);
    }

    public async Task<OperationResult<IEnumerable<ForumComment?>>> GetAllCommentsOfPostAsync(Guid postId)
    {
        var comments = await _commmentRepository.GetAllofPostAsync(postId);
        if (comments == null || !comments.Any())
            return OperationResult<IEnumerable<ForumComment?>>.Success(new List<ForumComment?>());

        return OperationResult<IEnumerable<ForumComment?>>.Success(comments);
    }

    public async Task<OperationResult<ForumComment>> UpdateCommentAsync(Guid commentId, Guid userId, ForumComment comment)
    {
        if (comment == null)
            return OperationResult<ForumComment>.Failure(CommentContentEmpty);

        var isAuthorized = await TestAuthorizationAsync(userId, commentId);
        if (!isAuthorized.IsSuccess)
            return OperationResult<ForumComment>.Failure(NotAuthorized);

        try
        {
            await _commmentRepository.UpdateAsync(commentId, comment);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ FULL EXCEPTION:");
            Console.WriteLine(ex.ToString());  // This gives you stack trace + inner exception
            return OperationResult<ForumComment>.Failure($"{UnknownError}: {ex}");
        }

        var result = await _commmentRepository.GetByIdAsync(commentId);
        return OperationResult<ForumComment>.Success(result!);
    }

    public async Task<OperationResult<bool>> DeleteCommentAsync(Guid commentId, Guid userId)
    {
        var isAuthorized = await TestAuthorizationAsync(userId, commentId);
        if (!isAuthorized.IsSuccess)
            return OperationResult<bool>.Failure(NotAuthorized);

        try
        {
            await _commmentRepository.DeleteAsync(commentId);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ FULL EXCEPTION:");
            Console.WriteLine(ex.ToString());  // This gives you stack trace + inner exception
            return OperationResult<bool>.Failure($"{UnknownError}: {ex}");
        }

        return OperationResult<bool>.Success(true);
    }

    private async Task<OperationResult<bool>> TestAuthorizationAsync(Guid userId, Guid commentId)
    {
        if (IsInvalid(commentId))
            return OperationResult<bool>.Failure(InvalidCommentId);

        if (IsInvalid(userId))
            return OperationResult<bool>.Failure(InvalidUserId);

        var post = await _commmentRepository.isAuthorizedAsync(userId, commentId);
        if (!post)
            return OperationResult<bool>.Failure(NotAuthorized);

        return OperationResult<bool>.Success(true);
    }

    private static bool IsInvalid(Guid id) => id == Guid.Empty;
}

