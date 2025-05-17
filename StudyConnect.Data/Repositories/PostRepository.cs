using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Models;
using StudyConnect.Core.Interfaces.Repositories;
using StudyConnect.Data.Utilities;

namespace StudyConnect.Data.Repositories;

public class PostRepository : BaseRepository, IPostRepository
{
    public PostRepository(StudyConnectDbContext context) : base(context) { }

    public async Task<Guid> AddAsync(ForumPost post, Guid userId, Guid categoryId)
    {
        var newPost = new Entities.ForumPost
        {
            Title = post.Title,
            Content = post.Content,
            UserId = userId,
            ForumCategoryId = categoryId
        };

        await _context.ForumPosts.AddAsync(newPost);
        await _context.SaveChangesAsync();

        return newPost.ForumPostId;
    }

    public async Task<IEnumerable<ForumPost>> SearchAsync(
        Guid? userId,
        string? categoryName,
        string? title,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var query = _context.ForumPosts
            .AsNoTracking()
            .WhereIf(userId.HasValue, p => p.UserId == userId)
            .WhereIf(!string.IsNullOrWhiteSpace(categoryName), p => p.ForumCategory.Name == categoryName)
            .WhereIf(!string.IsNullOrWhiteSpace(title), p => EF.Functions.Like(p.Title, $"%{title}%"))
            .WhereIf(fromDate.HasValue, p => p.CreatedAt >= fromDate!.Value.Date)
            .WhereIf(toDate.HasValue, p => p.CreatedAt <= toDate!.Value.Date);

        var posts = await query.ToListAsync();
        return posts
            .Select(p => p.ToForumPostModel());
        ;
    }

    public async Task<ForumPost?> GetByIdAsync(Guid postId)
    {
        var post = await _context.ForumPosts
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ForumPostId == postId);

        return post?.ToForumPostModel();
    }

    public async Task UpdateAsync(Guid postId, ForumPost updatedPost)
    {
        var existing = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == postId);
        if (existing == null) return;

        existing.Title = updatedPost.Title;
        existing.Content = updatedPost.Content;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid postId)
    {
        var post = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == postId);
        if (post == null) return;

        _context.ForumPosts.Remove(post);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid postId) =>
        await _context.ForumPosts.AnyAsync(p => p.ForumPostId == postId);

    public async Task<bool> TitleExistsAsync(string title) =>
        await _context.ForumPosts.AnyAsync(p => p.Title == title);

    public async Task<bool> ContainsUserAsync(Guid userId, Guid postId) =>
        await _context.ForumPosts.AnyAsync(p => p.ForumPostId == postId && p.UserId == userId);

    public async Task IncrementCommentCountAsync(Guid postId)
    {
        var post = await _context.ForumPosts.FindAsync(postId);
        if (post == null) return;

        post.CommentCount++;
        _context.Entry(post).Property(p => p.CommentCount).IsModified = true;

        await _context.SaveChangesAsync();
    }
}

