using StudyConnect.Core.Interfaces.Services;
using StudyConnect.Core.Interfaces.Repositories;
using StudyConnect.Core.Common;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.Services;

public class LikeService : ILikeService
{
    protected readonly IUserRepository _userRepository;
    protected readonly IPostRepository _postRepository;
    protected readonly ICommentRepository _commentRepository;
    protected readonly ILikeRepository _likeRepository;

    public LikeService(
        IUserRepository userRepository,
        IPostRepository postRepository,
        ICommentRepository commentRepository,
        ILikeRepository likeRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        _likeRepository = likeRepository ?? throw new ArgumentNullException(nameof(likeRepository));
    }

    public async Task<OperationResult<int>> GetLikeCountAsync(Guid? postId, Guid? commentId)
    {
        var count = 0;
        if (postId.HasValue == commentId.HasValue)
            return OperationResult<int>.Failure(InvalidInput);

        if (postId != null)
        {
            var pid = postId.Value;

            if (IsInvalid(pid))
                return OperationResult<int>.Failure(InvalidPostId);

            if (!await _postRepository.ExistsAsync(pid))
                return OperationResult<int>.Failure(PostNotFound);

            count = await _likeRepository.GetPostLikeCountAsync(pid);
        }

        if (commentId != null)
        {
            var cid = commentId.Value;

            if (IsInvalid(cid))
                return OperationResult<int>.Failure(InvalidCommentId);

            if (!await _commentRepository.ExistsAsync(cid))
                return OperationResult<int>.Failure(CommentNotFound);

            count = await _likeRepository.GetCommentLikeCountAsync(cid);
        }
        return OperationResult<int>.Success(count);
    }


    public async Task<OperationResult<bool>> LeaveLikeAsync(Guid userId, Guid? postId, Guid? commentId)
    {
        if (IsInvalid(userId) || !await _userRepository.UserExistsAsync(userId))
            return OperationResult<bool>.Failure(UserNotFound);

        if (postId.HasValue == commentId.HasValue)
            return OperationResult<bool>.Failure(InvalidInput);

        if (postId != null)
        {
            Guid pid = (Guid)postId;

            if (IsInvalid(pid) || !await _postRepository.ExistsAsync(pid))
                return OperationResult<bool>.Failure(PostNotFound);

            var alreadyLiked = await _likeRepository.PostLikeExistsAsync(userId, pid);
            if (alreadyLiked)
                return OperationResult<bool>.Failure(LikeExists);

            try
            {
                await _likeRepository.LikePostAsync(userId, pid);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"{UnknownError}: {ex}");
            }
        }

        if (commentId != null)
        {
            Guid cid = (Guid)commentId;

            if (IsInvalid(cid) || !await _commentRepository.ExistsAsync(cid))
                return OperationResult<bool>.Failure(CommentNotFound);

            var alreadyLiked = await _likeRepository.PostLikeExistsAsync(userId, cid);
            if (alreadyLiked)
                return OperationResult<bool>.Failure(LikeExists);

            try
            {
                await _likeRepository.LikeCommentAsync(userId, cid);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"{UnknownError}: {ex}");
            }

        }
        return OperationResult<bool>.Success(true);
    }

    public async Task<OperationResult<bool>> RemoveLikeAsync(Guid userId, Guid? postId, Guid? commentId)
    {
        if (IsInvalid(userId) || !await _userRepository.UserExistsAsync(userId))
            return OperationResult<bool>.Failure(UserNotFound);

        if (postId != null)
        {
            Guid pid = (Guid)postId;

            if (IsInvalid(pid) || !await _postRepository.ExistsAsync(pid))
                return OperationResult<bool>.Failure(PostNotFound);

            var LikeExists = await _likeRepository.PostLikeExistsAsync(userId, pid);
            if (!LikeExists)
                return OperationResult<bool>.Failure(LikeNotFound);

            try
            {
                await _likeRepository.UnlikePostAsync(userId, pid);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"{UnknownError}: {ex}");
            }
        }

        if (commentId != null)
        {
            Guid cid = (Guid)commentId;

            if (IsInvalid(cid) || !await _commentRepository.ExistsAsync(cid))
                return OperationResult<bool>.Failure(CommentNotFound);

            var LikeExists = await _likeRepository.PostLikeExistsAsync(userId, cid);
            if (!LikeExists)
                return OperationResult<bool>.Failure(LikeNotFound);

            try
            {
                await _likeRepository.UnlikeCommentAsync(userId, cid);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"{UnknownError}: {ex}");
            }
        }
        return OperationResult<bool>.Success(true);
    }

    private static bool IsInvalid(Guid id) => id == Guid.Empty;

}

