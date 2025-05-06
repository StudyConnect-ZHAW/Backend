using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Common;
using StudyConnect.Core.Models;
using StudyConnect.Core.Interfaces;

namespace StudyConnect.Data.Repositories;

public class PostRepository : BaseRepository, IPostRepository
{
    public PostRepository(StudyConnectDbContext context) : base(context)
    {

    }

    public async Task<OperationResult<Guid?>> AddAsync(Guid userId, Guid categoryId, ForumPost? post)
    {
        if (userId == Guid.Empty)
            return OperationResult<Guid?>.Failure("User Id is Invalid.");

        if (categoryId == Guid.Empty)
            return OperationResult<Guid?>.Failure("Category Id is Invalid.");

        if (post == null)
            return OperationResult<Guid?>.Failure("Post cannot be null.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserGuid == userId);
        var category = await _context.ForumCategories.FirstOrDefaultAsync(c => c.ForumCategoryId == categoryId);
        if (user == null || category == null)
            return OperationResult<Guid?>.Failure("User or Category not found.");

        var testTitle = await _context.ForumPosts.AnyAsync(fp => fp.Title == post.Title);
        if (testTitle)
            return OperationResult<Guid?>.Failure("Post Title already taken.");

        try
        {
            var newPost = new Entities.ForumPost
            {
                Title = post.Title,
                Content = post.Content,
                ForumCategory = category,
                User = user
            };

            await _context.ForumPosts.AddAsync(newPost);
            await _context.SaveChangesAsync();


            return OperationResult<Guid?>.Success(newPost.ForumPostId);
        }
        catch (Exception ex)
        {
            return OperationResult<Guid?>.Failure($"Failed to add the Post: {ex.Message}");
        }
    }

    public async Task<OperationResult<IEnumerable<ForumPost>?>> SearchAsync(Guid? userId, string? categoryName, string? title)
    {
        var posts = _context.ForumPosts
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .AsQueryable();

        if (userId.HasValue)
            posts = posts.Where(p => p.User.UserGuid == userId.Value);

        if (!string.IsNullOrEmpty(categoryName))
            posts = posts.Where(p => p.ForumCategory.Name == categoryName);

        if (!string.IsNullOrEmpty(title))
            posts = posts.Where(p => EF.Functions.Like(p.Title, $"%{title}%"));

        var queries = await posts.ToListAsync();

        if (queries.Count == 0)
            return OperationResult<IEnumerable<ForumPost>?>.Success(null);

        var result = queries.Select(p => PackagePost(p));

        return OperationResult<IEnumerable<ForumPost>?>.Success(result);
    }

    public async Task<OperationResult<IEnumerable<ForumPost>?>> GetAllAsync()
    {
        var posts = await _context.ForumPosts
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .ToListAsync();

        if (posts.Count == 0)
            return OperationResult<IEnumerable<ForumPost>?>.Success(null);

        var result = posts.Select(p => PackagePost(p));

        return OperationResult<IEnumerable<ForumPost>?>.Success(result);
    }

    public async Task<OperationResult<ForumPost?>> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return OperationResult<ForumPost?>.Failure("Invalid ForumPost ID.");

        var post = await _context.ForumPosts
            .Include(p => p.User)
            .Include(p => p.ForumCategory)
            .AsNoTracking()
            .FirstOrDefaultAsync(fp => fp.ForumPostId == id);


        if (post == null)
            return OperationResult<ForumPost?>.Success(null);

        var result = PackagePost(post);

        return OperationResult<ForumPost?>.Success(result);
    }

    public async Task<OperationResult<bool>> UpdateAsync(Guid id, ForumPost post)
    {
        if (id == Guid.Empty)
            return OperationResult<bool>.Failure("Invalid ForumPost ID.");

        if (post == null)
            return OperationResult<bool>.Failure("Post cannot be null.");

        var postToUpdate = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == id);
        if (postToUpdate == null)
            return OperationResult<bool>.Success(false);

        try
        {
            postToUpdate.Title = post.Title;
            postToUpdate.Content = post.Content;
            postToUpdate.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"An Error occurred while updating: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            return OperationResult<bool>.Failure("Invalid ForumPost ID.");

        var postToDelete = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == id);
        if (postToDelete == null)
            return OperationResult<bool>.Success(false);

        try
        {
            _context.Remove(postToDelete);
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"An Error occurred while deleting: {ex.Message}");
        }
    }

    /// <summary>
    /// A helper function to create a forum post model from entity.
    /// </summary>
    /// <param name="post">A forum post entity to transform.</param>
    /// <returns>A forum post model object.</returns>
    private ForumPost PackagePost(Entities.ForumPost post)
    {
        return new ForumPost
        {
            ForumPostId = post.ForumPostId,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            Category = PackageCategory(post.ForumCategory),
            User = PackageUser(post.User)
        };
    }

    /// <summary>
    /// A helper function to create a category model from entity.
    /// </summary>
    /// <param name="category">A category entity to transform.</param>
    /// <returns>A forumcategory model object.</returns>
    private ForumCategory PackageCategory(Entities.ForumCategory category)
    {
        return new ForumCategory
        {
            ForumCategoryId = category.ForumCategoryId,
            Name = category.Name,
            Description = category.Description
        };
    }

    /// <summary>
    /// A helper function to create a user model from entity.
    /// </summary>
    /// <param name="user">An user entity to transform.</param>
    /// <returns>An User model object.</returns>
    private User PackageUser(Entities.User user)
    {
        return new User
        {
            UserGuid = user.UserGuid,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }
}
