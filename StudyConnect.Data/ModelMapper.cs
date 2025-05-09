namespace StudyConnect.Data;

public static class ModelMapper
{
    /// <summary>
    /// A helper function to create a user model from entity.
    /// </summary>
    /// <param name="user">An user entity to transform.</param>
    /// <returns>An User model object.</returns>
    public static Core.Models.User PackageUser(Entities.User user)
    {
        return new Core.Models.User
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
    public static Core.Models.ForumCategory PackageCategory(Entities.ForumCategory category)
    {
        return new Core.Models.ForumCategory
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
    public static Core.Models.ForumPost PackagePost(Entities.ForumPost post)
    {
        return new Core.Models.ForumPost
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
    public static Core.Models.ForumComment PackageComment(Entities.ForumComment comment)
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
    public static Core.Models.ForumComment PackageCommentTree(Entities.ForumComment comment)
    {
        var result = PackageCommentBase(comment);

        result.ParentComment = null;
        result.Replies = comment.Replies?
            .Select(c => PackageCommentTree(c))
            .ToList() ?? null;

        return result;
    }

    /// <summary>
    /// A function containing commentt properies which can be used by PackageComment and PackageCommentTree.
    /// </summary>
    /// <param name="comment">A comment entity to transform.</param>
    /// <returns>A forum comment model object.</returns>
    public static Core.Models.ForumComment PackageCommentBase(Entities.ForumComment comment)
    {
        return new Core.Models.ForumComment
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
