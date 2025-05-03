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

    public async Task<OperationResult<bool>> AddAsync(ForumPost? post)
    {
        if (post == null)
            return OperationResult<bool>.Failure("Post cannot be null.");

        var testTitle = await _context.ForumPosts.FirstOrDefaultAsync(fp => fp.Title == post.Title);
        if (testTitle != null)
            return OperationResult<bool>.Failure("Post Title already taken.");

        try
        {
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.URoleId == Guid.Parse("00000000-0000-0000-0000-000000000001"));
            if (userRole == null)
                return OperationResult<bool>.Failure("User Role not found");

            var author = extractUser(post.User, userRole);

            var category = extractCategory(post.Category);

            var newPost = new Entities.ForumPost
            {
                ForumPostId = Guid.NewGuid(),
                Title = post.Title,
                Content = post.Content,
                ForumCategory = category,
                User = author
            };

            await _context.ForumPosts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"Failed to add the Post: {ex.Message}");
        }
    }

    public async Task<OperationResult<IEnumerable<ForumPost>>> GetAllAsync()
    {
        var posts = await _context.ForumPosts
            .AsNoTracking()
            .ToListAsync();

        if (posts.Count == 0)
            return OperationResult<IEnumerable<ForumPost>>.Failure("No Posts were found.");

        var result = posts.Select(p =>
        {
            var userModel = packageUser(p.User);
            var categoryModel = packageCatergory(p.ForumCategory);
            return new ForumPost
            {
                Title = p.Title,
                Content = p.Content,
                Category = categoryModel,
                User = userModel
            };
        });

        return OperationResult<IEnumerable<ForumPost>>.Success(result);
    }

    public async Task<OperationResult<ForumPost?>> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return OperationResult<ForumPost?>.Failure("Invalid ForumPost ID.");

        var forumPost = await _context.ForumPosts
            .AsNoTracking()
            .FirstOrDefaultAsync(fp => fp.ForumPostId == id);

        if (forumPost == null)
            return OperationResult<ForumPost?>.Failure("ForumPost not found.");

        var userModel = packageUser(forumPost.User);

        var categoryModel = packageCatergory(forumPost.ForumCategory);
        var result = new ForumPost
        {
            Title = forumPost.Title,
            Content = forumPost.Content,
            Category = categoryModel,
            User = userModel
        };

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
            return OperationResult<bool>.Failure("ForumPost not found.");

        try
        {
            postToUpdate.Title = post.Title;
            postToUpdate.Content = post.Content;

            await _context.SaveChangesAsync();

            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"An Error occured while updateting: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            return OperationResult<bool>.Failure("Invalid ForumPost ID.");

        var postToDelete = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == id);
        if (postToDelete == null)
            return OperationResult<bool>.Failure("Post could not be found.");

        _context.Remove(postToDelete);
        await _context.SaveChangesAsync();

        return OperationResult<bool>.Success(true);
    }

    /// <summary>
    /// A helper function to extract an category entity from a model.
    /// </summary>
    /// <param name="category">A category model to extract.</param>
    /// <returns>An forumcategory entity that can be stored in the database.</returns>
    private Entities.ForumCategory extractCategory(ForumCategory category)
    {
        return new Entities.ForumCategory
        {
            ForumCategoryId = category.ForumCategoryId,
            Name = category.Name,
            Description = category.Description
        };
    }

    /// <summary>
    /// A helper function to create a category model from entity.
    /// </summary>
    /// <param name="category">A category entity to create. </param>
    /// <returns>A forumcategory model object.</returns>
    private ForumCategory packageCatergory(Entities.ForumCategory category)
    {
        return new ForumCategory
        {
            ForumCategoryId = category.ForumCategoryId,
            Name = category.Name,
            Description = category.Description
        };
    }

    /// <summary>
    /// A helper function to extract an user entity from a model.
    /// </summary>
    /// <param name="user">An user model to extract.</param>
    /// <param name="role">A user role needed to create a user entity<./param>
    /// <returns>An user entity that can be stored in the database.</returns>
    private Entities.User extractUser(User user, Entities.UserRole role)
    {
        return new Entities.User
        {
            UserGuid = user.UserGuid,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            URole = role
        };
    }

    /// <summary>
    /// A helper function to create a user model from entity.
    /// </summary>
    /// <param name="user">An user entity to create. </param>
    /// <returns>An User model object.</returns>
    private User packageUser(Entities.User user)
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
