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

    public async Task<bool> TestForTitleAsync(string title) =>
        await _context.ForumPosts.AnyAsync(fp => fp.Title == title);

    public async Task<ForumPost> AddAsync(ForumPost post)
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

        return await GetByIdAsync(newPost.ForumPostId);
    }

    public async Task<IEnumerable<ForumPost>?> SearchAsync(
        Guid? userId,
        string? categoryName,
        string? title,
        DateTime? fromDate,
        DateTime? toDate
    )
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

        return posts.Select(p => p.MapToForumPost());
        ;
    }

    public async Task<ForumPost?> GetByIdAsync(Guid id)
    {
        var post = await _context.ForumPosts
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .AsNoTracking()
            .FirstOrDefaultAsync(fp => fp.ForumPostId == id);

        return post!.MapToForumPost();
    }

    public async Task UpdateAsync(ForumPost post)
    {
        var toUpdate = post.MapFromForumPost(null, null);

        _context.ForumPosts.Update(toUpdate);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ForumPost post)
    {
        var toDelete = post.MapFromForumPost(null, null);

        _context.Remove(post);
        await _context.SaveChangesAsync();
    }
}
