using StudyConnect.Core.Interfaces.Services;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Common;
using StudyConnect.Core.Models;

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
            return OperationResult<ForumPost>.Failure("User not found.");

        if (IsInvalid(categoryId))
            return OperationResult<ForumPost>.Failure("category not found.");

        if (post == null)
            return OperationResult<ForumPost>.Failure("Post cannot be null.");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user.Data == null)
            return OperationResult<ForumPost>.Failure("user not found.");

        var category = await _categoryRepository.CategoryExistAsync(categoryId);
        if (!category)
            return OperationResult<ForumPost>.Failure("category not found.");

        var isTitleTaken = await _postRepository.TitleExistsAsync(post.Title);
        if (isTitleTaken)
            return OperationResult<ForumPost>.Failure("Title already taken.");

        try
        {
            var finalpost = new ForumPost
            {
                Title = post.Title,
                Content = post.Content,
                UserId = userId,
                ForumCategoryId = categoryId
            };

            var resultId = await _postRepository.AddAsync(finalpost);
            var result = await _postRepository.GetByIdAsync(resultId, false);
            return OperationResult<ForumPost>.Success(result!);

        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ FULL EXCEPTION:");
            Console.WriteLine(ex.ToString());  // This gives you stack trace + inner exception

            return OperationResult<ForumPost>.Failure($"Failed to add the Post: {ex}");
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
            return OperationResult<IEnumerable<ForumPost>>.Failure("Nothing was found");

        return OperationResult<IEnumerable<ForumPost>>.Success(result);
    }

    public async Task<OperationResult<ForumPost>> GetPostByIdAsync(Guid postId)
    {
        if (IsInvalid(postId))
            return OperationResult<ForumPost>.Failure("Invalid Id");

        var result = await _postRepository.GetByIdAsync(postId, false);
        if (result == null)
            return OperationResult<ForumPost>.Failure("Post not found.");

        return OperationResult<ForumPost>.Success(result);
    }

    public async Task<OperationResult<bool>> UpdatePostAsync(Guid userId, Guid postId, ForumPost post)
    {
        if (post == null)
            return OperationResult<bool>.Failure("post should not be Empty.");

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
            return OperationResult<bool>.Failure($"An error occurred while updating: {ex}");
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
            return OperationResult<bool>.Failure($"An error occurred while updating: {ex}");
        }
    }

    private static bool IsInvalid(Guid id) => id == Guid.Empty;

    private async Task<OperationResult<bool>> TestAuthorizationAsync(Guid userId, Guid postId)
    {
        if (IsInvalid(postId))
            return OperationResult<bool>.Failure("Invalid postId.");

        if (IsInvalid(userId))
            return OperationResult<bool>.Failure("Invalid userId.");

        var post = await _postRepository.isAthorizedAsync(userId, postId);
        if (!post)
            return OperationResult<bool>.Failure("Post not Authorized.");

        return OperationResult<bool>.Success(true);
    }
}

