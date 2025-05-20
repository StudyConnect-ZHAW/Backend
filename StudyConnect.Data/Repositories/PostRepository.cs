using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Common;
using StudyConnect.Core.Models;
using StudyConnect.Core.Interfaces;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.Data.Repositories;

public class PostRepository : BaseRepository, IPostRepository
{
    public PostRepository(StudyConnectDbContext context) : base(context) { }

    public async Task<OperationResult<ForumPost>> AddAsync(Guid userId, Guid categoryId, ForumPost? post)
    {
        if (!await IsValidUser(userId))
            return OperationResult<ForumPost>.Failure(UserNotFound);

        if (!await IsValidCategory(categoryId))
            return OperationResult<ForumPost>.Failure(CategoryNotFound);

        if (post == null)
            return OperationResult<ForumPost>.Failure(PostContentEmpty);

        bool isTitleTaken = await _context.ForumPosts.AnyAsync(fp => fp.Title == post.Title);
        if (isTitleTaken)
            return OperationResult<ForumPost>.Failure(TitleTaken);

        var newPost = new Entities.ForumPost
        {
            Title = post.Title,
            Content = post.Content,
            ForumCategoryId = categoryId,
            UserId = userId
        };

        try
        {
            await _context.ForumPosts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            var created = await _context.ForumPosts
                .Include(p => p.User)
                .Include(p => p.ForumCategory)
                .FirstOrDefaultAsync(p => p.ForumPostId == newPost.ForumPostId);

            if (created is null)
                return OperationResult<ForumPost>.Failure($"{UnknownError}: Failed to retrieve the newly created post.");

            return OperationResult<ForumPost>.Success(MapToPostModel(created));

        }
        catch (Exception ex)
        {
            return OperationResult<ForumPost>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<IEnumerable<ForumPost>>> SearchAsync(
        Guid? userId,
        string? categoryName,
        string? title, DateTime?
        fromDate,
        DateTime? toDate
    )
    {
        var query = _context.ForumPosts
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .Include(p => p.ForumComments)
            .Include(p => p.ForumLikes)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(p => p.User.UserGuid == userId.Value);

        if (!string.IsNullOrWhiteSpace(categoryName))
            query = query.Where(p => p.ForumCategory.Name == categoryName);

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(p => EF.Functions.Like(p.Title, $"%{title}%"));

        if (fromDate.HasValue)
            query = query.Where(p => p.CreatedAt >= fromDate);

        if (toDate.HasValue)
            query = query.Where(p => p.CreatedAt <= toDate);

        var posts = await query.ToListAsync();
        var result = posts.Select(MapToPostModel);


        return OperationResult<IEnumerable<ForumPost>>.Success(result);
    }

    public async Task<OperationResult<IEnumerable<ForumPost>>> GetAllAsync()
    {

        var posts = await _context.ForumPosts
              .AsNoTracking()
              .Include(p => p.User)
              .Include(p => p.ForumCategory)
              .Include(p => p.ForumLikes)
              .Include(p => p.ForumComments)
              .ToListAsync();

        var result = posts.Select(MapToPostModel);
        return OperationResult<IEnumerable<ForumPost>>.Success(result);
    }

    public async Task<OperationResult<ForumPost?>> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return OperationResult<ForumPost?>.Failure(InvalidPostId);

        var post = await _context.ForumPosts
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .Include(p => p.ForumComments)
            .FirstOrDefaultAsync(p => p.ForumPostId == id);

        return post == null
            ? OperationResult<ForumPost?>.Failure(PostNotFound)
            : OperationResult<ForumPost?>.Success(MapToPostModel(post));
    }

    public async Task<OperationResult<ForumPost>> UpdateAsync(Guid userId, Guid postId, ForumPost post)
    {
        if (userId == Guid.Empty)
            return OperationResult<ForumPost>.Failure(InvalidUserId);

        if (postId == Guid.Empty)
            return OperationResult<ForumPost>.Failure(InvalidPostId);

        var (result, error) = await GetAuthorizedPostAsync(userId, postId);
        if (result == null)
            return OperationResult<ForumPost>.Failure(error ?? NotAuthorized);

        result.Title = post.Title;
        result.Content = post.Content;
        result.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
            return OperationResult<ForumPost>.Success(MapToPostModel(result));
        }
        catch (Exception ex)
        {
            return OperationResult<ForumPost>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid userId, Guid postId)
    {
        if (userId == Guid.Empty)
            return OperationResult<bool>.Failure(InvalidUserId);

        if (postId == Guid.Empty)
            return OperationResult<bool>.Failure(InvalidPostId);

        var (result, error) = await GetAuthorizedPostAsync(userId, postId);
        if (result == null)
            return OperationResult<bool>.Failure(error ?? NotAuthorized);

        try
        {
            _context.ForumPosts.Remove(result);
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    /// <summary>
    /// Validates if a user exists in the database.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns><c>true</c> if the user exists; otherwise, <c>false</c>.</returns>
    private async Task<bool> IsValidUser(Guid userId) =>
        userId != Guid.Empty && await _context.Users.AnyAsync(u => u.UserGuid == userId);

    /// <summary>
    /// Validates if a category exists in the database.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category.</param>
    /// <returns><c>true</c> if the category exists; otherwise, <c>false</c>.</returns>
    private async Task<bool> IsValidCategory(Guid categoryId) =>
        categoryId != Guid.Empty && await _context.ForumCategories.AnyAsync(c => c.ForumCategoryId == categoryId);

    /// <summary>
    /// Tests a post for existence and authorization.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns>A post entity on succes or an errormessage on failure.</returns>
    private async Task<(Entities.ForumPost? Post, string? ErrorMessage)> GetAuthorizedPostAsync(Guid userId, Guid postId)
    {
        var post = await _context.ForumPosts
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .Include(p => p.ForumLikes)
            .FirstOrDefaultAsync(c => c.ForumPostId == postId);

        if (post == null)
            return (null, PostNotFound);

        if (post.UserId != userId)
            return (null, NotAuthorized);

        return (post, null);
    }

    /// <summary>
    /// Maps the post entity to a model.
    /// </summary>
    /// <param name="post">The post entity to map.</param>
    /// <returns>A <see cref="ForumPost"/> </returns>
    private ForumPost MapToPostModel(Entities.ForumPost post) => new()
    {
        ForumPostId = post.ForumPostId,
        Title = post.Title,
        Content = post.Content,
        CreatedAt = post.CreatedAt,
        UpdatedAt = post.UpdatedAt,
        Category = post.ForumCategory.ToCategoryModel(),
        User = post.User.ToUserModel(),
        LikeCount = post.ForumLikes.Count,
        CommentCount = post.ForumComments.Count
    };
}

