using StudyConnect.Core.Interfaces.Services;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Common;
using StudyConnect.Core.Models;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.Services;

public class PostService : IPostService
{
    protected readonly IUserRepository _userRepository;
    protected readonly ICategoryRepository _categoryRepository;
    protected readonly IPostRepository _postRepository;

    public PostService(
        IUserRepository userRepository,
        ICategoryRepository categoryRepository,
        IPostRepository postRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
    }

    public async Task<OperationResult<ForumPost>> AddPostAsync(Guid userId, Guid categoryId, ForumPost post)
    {
        if (IsInvalid(userId))
            return OperationResult<ForumPost>.Failure(UserNotFound);

        if (IsInvalid(categoryId))
            return OperationResult<ForumPost>.Failure(CategoryNotFound);

        if (post == null)
            return OperationResult<ForumPost>.Failure(PostConentEmpty);

        var user = await _userRepository.GetByIdAsync(userId);
        if (user.Data == null)
            return OperationResult<ForumPost>.Failure(PostNotFound);

        var category = await _categoryRepository.CategoryExistAsync(categoryId);
        if (!category)
            return OperationResult<ForumPost>.Failure(CategoryNotFound);

        var isTitleTaken = await _postRepository.TitleExistsAsync(post.Title);
        if (isTitleTaken)
            return OperationResult<ForumPost>.Failure(TitleTaken);

        try
        {
            var resultId = await _postRepository.AddAsync(post, userId, categoryId);
            var result = await _postRepository.GetByIdAsync(resultId);
            return OperationResult<ForumPost>.Success(result!);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ FULL EXCEPTION:");
            Console.WriteLine(ex.ToString());  // This gives you stack trace + inner exception

            return OperationResult<ForumPost>.Failure($"{UnknownError}: {ex}");
        }
    }

    public async Task<OperationResult<IEnumerable<ForumPost>>> SearchPostAsync(
        Guid? userId,
        string? categoryName,
        string? title,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var result = await _postRepository.SearchAsync(userId, categoryName, title, fromDate, toDate);
        if (result == null || !result.Any())
            return OperationResult<IEnumerable<ForumPost>>.Failure(NotFound);

        return OperationResult<IEnumerable<ForumPost>>.Success(result);
    }

    public async Task<OperationResult<ForumPost>> GetPostByIdAsync(Guid postId)
    {
        if (IsInvalid(postId))
            return OperationResult<ForumPost>.Failure(InvalidPostId);

        var result = await _postRepository.GetByIdAsync(postId);
        if (result == null)
            return OperationResult<ForumPost>.Failure(PostNotFound);

        return OperationResult<ForumPost>.Success(result);
    }

    public async Task<OperationResult<bool>> UpdatePostAsync(Guid userId, Guid postId, ForumPost post)
    {
        if (post == null)
            return OperationResult<bool>.Failure(PostConentEmpty);

        var test = await TestAuthorizationAsync(userId, postId);
        if (!test.IsSuccess)
            return OperationResult<bool>.Failure(test.ErrorMessage!);

        try
        {
            await _postRepository.UpdateAsync(postId, post);
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ FULL EXCEPTION:");
            Console.WriteLine(ex.ToString());
            return OperationResult<bool>.Failure($"{UnknownError}: {ex}");
        }
    }

    public async Task<OperationResult<bool>> DeletePostAsync(Guid userId, Guid postId)
    {
        var actualPost = await TestAuthorizationAsync(userId, postId);
        if (!actualPost.IsSuccess)
            return OperationResult<bool>.Failure(actualPost.ErrorMessage!);

        try
        {
            await _postRepository.DeleteAsync(postId);
            return OperationResult<bool>.Success(true);

        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ FULL EXCEPTION:");
            Console.WriteLine(ex.ToString());
            return OperationResult<bool>.Failure($"{UnknownError}: {ex}");
        }
    }

    private static bool IsInvalid(Guid id) => id == Guid.Empty;

    private async Task<OperationResult<bool>> TestAuthorizationAsync(Guid userId, Guid postId)
    {
        if (IsInvalid(postId))
            return OperationResult<bool>.Failure(InvalidPostId);

        if (IsInvalid(userId))
            return OperationResult<bool>.Failure(InvalidUserId);

        var post = await _postRepository.isAthorizedAsync(userId, postId);
        if (!post)
            return OperationResult<bool>.Failure(NotAuthorized);

        return OperationResult<bool>.Success(true);
    }
}

