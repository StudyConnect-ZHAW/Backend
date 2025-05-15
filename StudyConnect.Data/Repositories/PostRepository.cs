using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Models;
using StudyConnect.Core.Interfaces;
using StudyConnect.Data.Utilities;
namespace StudyConnect.Data.Repositories;

public class PostRepository : BaseRepository, IPostRepository
{
    public PostRepository(StudyConnectDbContext context) : base(context)
    {

    }

    public async Task<Guid> AddAsync(ForumPost post)
    {
        var newPost = new Entities.ForumPost
        {
            Title = post.Title,
            Content = post.Content,
            ForumCategoryId = post.ForumCategoryId!,
            UserId = post.UserId
        };

        await _context.ForumPosts.AddAsync(newPost);
        await _context.SaveChangesAsync();

        var result = newPost.ForumPostId;

        return result;
    }

    public async Task<IEnumerable<ForumPost>?> SearchAsync(
        Guid? userId,
        string? categoryName,
        string? title,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var query = _context.ForumPosts
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .WhereIf(userId.HasValue, p => p.User.UserId == userId)
            .WhereIf(!string.IsNullOrWhiteSpace(categoryName), p => p.ForumCategory.Name == categoryName)
            .WhereIf(!string.IsNullOrWhiteSpace(title), p => EF.Functions.Like(p.Title, $"%{title}%"))
            .WhereIf(fromDate.HasValue, p => p.CreatedAt >= fromDate!.Value.Date)
            .WhereIf(toDate.HasValue, p => p.CreatedAt <= toDate!.Value.Date);

        var posts = await query.ToListAsync();

        return posts.Select(p => p.ToForumPostModel(false));
        ;
    }

    public async Task<ForumPost?> GetByIdAsync(Guid id, bool Update)
    {
        var post = await _context.ForumPosts
            .AsNoTracking()
            .FirstOrDefaultAsync(fp => fp.ForumPostId == id);

        return post!.ToForumPostModel(Update);
    }

    public async Task UpdateAsync(Guid postId, ForumPost post)
    {
        var toUpdate = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == postId);

        toUpdate!.Title = post.Title;
        toUpdate!.Content = post.Content;

        _context.ForumPosts.Update(toUpdate);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid postId)
    {
        var toDelete = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == postId);

        _context.Remove(toDelete!);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> TitleExistsAsync(string title) =>
        await _context.ForumPosts.AnyAsync(p => p.Title == title);

    public async Task<bool> isAthorizedAsync(Guid userId, Guid postId) =>
        await _context.ForumPosts.AnyAsync(p => p.ForumPostId == postId && p.UserId == userId);
}
