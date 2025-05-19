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

            return OperationResult<ForumPost>.Success(MapPostToModel(created));

        }
        catch (Exception ex)
        {
            // Consider logging this instead of printing
            Console.WriteLine(ex);
            return OperationResult<ForumPost>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<IEnumerable<ForumPost>>> SearchAsync(Guid? userId, string? categoryName, string? title)
    {
        var query = _context.ForumPosts
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .Include(p => p.ForumLikes)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(p => p.User.UserGuid == userId.Value);

        if (!string.IsNullOrWhiteSpace(categoryName))
            query = query.Where(p => p.ForumCategory.Name == categoryName);

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(p => EF.Functions.Like(p.Title, $"%{title}%"));

        var posts = await query.ToListAsync();
        var result = posts.Select(MapPostToModel);

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

        var result = posts.Select(MapPostToModel);
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
            .FirstOrDefaultAsync(p => p.ForumPostId == id);

        return post == null
            ? OperationResult<ForumPost?>.Failure(PostNotFound)
            : OperationResult<ForumPost?>.Success(MapPostToModel(post));
    }

    public async Task<OperationResult<ForumPost>> UpdateAsync(Guid userId, Guid postId, ForumPost post)
    {
        if (!await IsValidUser(userId))
            return OperationResult<ForumPost>.Failure(UserNotFound);

        if (!await IsValidPost(postId))
            return OperationResult<ForumPost>.Failure(PostNotFound);

        var existing = await _context.ForumPosts
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .Include(p => p.ForumLikes)
            .FirstOrDefaultAsync(p => p.ForumPostId == postId && p.UserId == userId);

        if (existing == null)
            return OperationResult<ForumPost>.Failure(NotAuthorized);

        existing.Title = post.Title;
        existing.Content = post.Content;
        existing.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
            return OperationResult<ForumPost>.Success(MapPostToModel(existing));
        }
        catch (Exception ex)
        {
            return OperationResult<ForumPost>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid userId, Guid postId)
    {
        if (!await IsValidUser(userId))
            return OperationResult<bool>.Failure(UserNotFound);

        if (!await IsValidPost(postId))
            return OperationResult<bool>.Failure(PostNotFound);

        var post = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == postId && p.UserId == userId);
        if (post == null)
            return OperationResult<bool>.Failure(NotAuthorized);

        try
        {
            _context.ForumPosts.Remove(post);
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"{UnknownError}: {ex.Message}");
        }
    }

    // -------------------- Helper Methods --------------------

    private async Task<bool> IsValidUser(Guid userId) =>
        userId != Guid.Empty && await _context.Users.AnyAsync(u => u.UserGuid == userId);

    private async Task<bool> IsValidCategory(Guid categoryId) =>
        categoryId != Guid.Empty && await _context.ForumCategories.AnyAsync(c => c.ForumCategoryId == categoryId);

    private async Task<bool> IsValidPost(Guid postId) =>
        postId != Guid.Empty && await _context.ForumPosts.AnyAsync(p => p.ForumPostId == postId);

    public static ForumPost MapPostToModel(Entities.ForumPost post) => new()
    {
        ForumPostId = post.ForumPostId,
        Title = post.Title,
        Content = post.Content,
        CreatedAt = post.CreatedAt,
        UpdatedAt = post.UpdatedAt,
        Category = post.ForumCategory.ToCategoryModel(),
        User = post.User.ToUserModel(),
        LikeList = post.ForumLikes.Select(l => l.LikeId) ?? []
    };
}

