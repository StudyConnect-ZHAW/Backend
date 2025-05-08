using StudyConnect.Core.Models;

namespace StudyConnect.Data.Repositories;

public class BaseRepository
{
    protected readonly StudyConnectDbContext _context;

    public BaseRepository(StudyConnectDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// A helper function to create a user model from entity.
    /// </summary>
    /// <param name="user">An user entity to transform.</param>
    /// <returns>An User model object.</returns>
    protected User PackageUser(Entities.User user)
    {
        return new User
        {
            UserGuid = user.UserGuid,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    /// <summary>
    /// A helper function to create a category model from entity.
    /// </summary>
    /// <param name="category">A category entity to transform.</param>
    /// <returns>A forumcategory model object.</returns>
    protected ForumCategory PackageCategory(Entities.ForumCategory category)
    {
        return new ForumCategory
        {
            ForumCategoryId = category.ForumCategoryId,
            Name = category.Name,
            Description = category.Description
        };
    }

    /// <summary>
    /// A helper function to create a forum post model from entity.
    /// </summary>
    /// <param name="post">A forum post entity to transform.</param>
    /// <returns>A forum post model object.</returns>
    protected ForumPost PackagePost(Entities.ForumPost post)
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
    /// A helper function to create a forum comment model from entity.
    /// </summary>
    /// <param name="post">A forum comment entity to transform.</param>
    /// <returns>A forum comment model object.</returns>
    protected ForumComment PackageComment(Entities.ForumComment comment)
    {
        var result = PackageCommentBase(comment);

        result.ParentComment = comment.ParentComment != null
            ? PackageComment(comment.ParentComment)
            : null;

        return result;
    }

    /// <summary>
    /// A helper function to create a forum comment model from entity for the tree output.
    /// </summary>
    /// <param name="post">A forum comment entity to transform.</param>
    /// <returns>A forum comment model object.</returns>
    protected ForumComment PackageCommentTree(Entities.ForumComment comment)
    {
        var result = PackageCommentBase(comment);

        result.ParentComment = null;
        result.ChildComments = comment.Replies?
            .Select(c => PackageCommentTree(c))
            .ToList() ?? null;

        return result;
    }

    /// <summary>
    /// A function containing commentt properies which can be used by PackageComment and PackageCommentTree.
    /// </summary>
    /// <param name="comment">A comment entity to transform.</param>
    /// <returns>A forum comment model object.</returns>
    private ForumComment PackageCommentBase(Entities.ForumComment comment)
    {
        return new ForumComment
        {
            ForumcommentId = comment.ForumCommentId,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            ReplyCount = comment.ReplyCount,
            IsEdited = comment.IsEdited,
            isDeleted = comment.IsDeleted,
            Post = PackagePost(comment.ForumPost),
            User = PackageUser(comment.User),
        };
    }
}
