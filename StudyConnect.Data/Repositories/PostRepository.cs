using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Common;
using StudyConnect.Core.Models;
using StudyConnect.Core.Interfaces;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.Data.Repositories;

public class PostRepository : BaseRepository, IPostRepository
{
    public PostRepository(StudyConnectDbContext context) : base(context)
    {

    }

    public async Task<OperationResult<ForumPost>> AddAsync(Guid userId, Guid categoryId, ForumPost? post)
    {
        if (userId == Guid.Empty || !await TestForUser(userId))
            return OperationResult<ForumPost>.Failure(UserNotFound);

        if (categoryId == Guid.Empty || !await TestForCategory(categoryId))
            return OperationResult<ForumPost>.Failure(CategoryNotFound);

        if (post == null)
            return OperationResult<ForumPost>.Failure(PostContentEmpty);

        var testTitle = await _context.ForumPosts.AnyAsync(fp => fp.Title == post.Title);
        if (testTitle)
            return OperationResult<ForumPost>.Failure(TitleTaken);

        try
        {
            var newPost = new Entities.ForumPost
            {
                Title = post.Title,
                Content = post.Content,
                ForumCategoryId = categoryId,
                UserId = userId
            };

            await _context.ForumPosts.AddAsync(newPost);
            await _context.SaveChangesAsync();


            return OperationResult<ForumPost>.Success(MapPostToModel(newPost));
        }
        catch (Exception ex)
        {
            return OperationResult<ForumPost>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<IEnumerable<ForumPost>>> SearchAsync(Guid? userId, string? categoryName, string? title)
    {
        var posts = _context.ForumPosts
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .Include(p => p.ForumLikes)
            .AsQueryable();

        if (userId.HasValue)
            posts = posts.Where(p => p.User.UserGuid == userId.Value);

        if (!string.IsNullOrEmpty(categoryName))
            posts = posts.Where(p => p.ForumCategory.Name == categoryName);

        if (!string.IsNullOrEmpty(title))
            posts = posts.Where(p => EF.Functions.Like(p.Title, $"%{title}%"));

        var queries = await posts.ToListAsync();

        if (queries.Count == 0)
            return OperationResult<IEnumerable<ForumPost>>.Success([]);

        var result = queries.Select(p => MapPostToModel(p));

        return OperationResult<IEnumerable<ForumPost>>.Success(result);
    }

    public async Task<OperationResult<IEnumerable<ForumPost>>> GetAllAsync()
    {
        var posts = await _context.ForumPosts
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .Include(p => p.ForumLikes)
            .ToListAsync();

        if (posts.Count == 0)
            return OperationResult<IEnumerable<ForumPost>>.Success([]);

        var result = posts.Select(p => MapPostToModel(p));

        return OperationResult<IEnumerable<ForumPost>>.Success(result);
    }

    public async Task<OperationResult<ForumPost?>> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return OperationResult<ForumPost?>.Failure(InvalidPostId);

        var post = await _context.ForumPosts
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .AsNoTracking()
            .FirstOrDefaultAsync(fp => fp.ForumPostId == id);

        if (post == null)
            return OperationResult<ForumPost?>.Failure(PostNotFound);

        var result = MapPostToModel(post);

        return OperationResult<ForumPost?>.Success(result);
    }

    public async Task<OperationResult<ForumPost>> UpdateAsync(Guid userId, Guid postid, ForumPost post)
    {
        if (userId == Guid.Empty || await TestForUser(userId))
            return OperationResult<ForumPost>.Failure(UserNotFound);

        if (postid == Guid.Empty || await TestForPost(postid))
            return OperationResult<ForumPost>.Failure(PostNotFound);

        if (post == null)
            return OperationResult<ForumPost>.Failure(PostContentEmpty);

        var result = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == postid && p.UserId == userId);
        if (result == null)
            return OperationResult<ForumPost>.Failure(NotAuthorized);

        try
        {
            result.Title = post.Title;
            result.Content = post.Content;
            result.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return OperationResult<ForumPost>.Success(MapPostToModel(result));
        }
        catch (Exception ex)
        {
            return OperationResult<ForumPost>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid userId, Guid postid)
    {
        if (userId == Guid.Empty || await TestForUser(userId))
            return OperationResult<bool>.Failure(UserNotFound);

        if (postid == Guid.Empty || await TestForPost(postid))
            return OperationResult<bool>.Failure(PostNotFound);

        var result = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == postid && p.UserId == userId);
        if (result == null)
            return OperationResult<bool>.Failure(NotAuthorized);

        try
        {
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    /// <summary>
    /// Tests if the user exists in the database.
    /// </summary>
    /// <param name="postId">The unique idettifier of the user.</param>
    /// <returns><c>true</c> if the post exists; otherwise, <c>false</c>.</returns>
    private async Task<bool> TestForUser(Guid userId) =>
    await _context.Users.AnyAsync(u => u.UserGuid == userId);

    /// <summary>
    /// Tests if the category exists in the database.
    /// </summary>
    /// <param name="postId">The unique idettifier of the category.</param>
    /// <returns><c>true</c> if the post exists; otherwise, <c>false</c>.</returns>
    private async Task<bool> TestForCategory(Guid categoryId) =>
        await _context.ForumCategories.AnyAsync(c => c.ForumCategoryId == categoryId);

    /// <summary>
    /// Tests if the post exists in the database.
    /// </summary>
    /// <param name="postId">The unique idettifier of the post.</param>
    /// <returns><c>true</c> if the post exists; otherwise, <c>false</c>.</returns>
    private async Task<bool> TestForPost(Guid postid) =>
        await _context.ForumPosts.AnyAsync(p => p.ForumPostId == postid);

    /// <summary>
    /// A helper function to create a forum post model from entity.
    /// </summary>
    /// <param name="post">A forum post entity to transform.</param>
    /// <returns>A forum post model object.</returns>
    public static Core.Models.ForumPost MapPostToModel(Entities.ForumPost post)
    {
        return new Core.Models.ForumPost
        {
            ForumPostId = post.ForumPostId,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            Category = post.ForumCategory.ToCategoryModel(),
            User = post.User.ToUserModel(),
            LikeList = post.ForumLikes.Select(l => l.LikeId)
        };
    }
}
