namespace StudyConnect.Data.Utilities;

public static class ModelMappers
{
    public static Core.Models.UserRole MapToUserRole(this Entities.UserRole userRole)
    {
        return new Core.Models.UserRole
        {
            URoleId = userRole.URoleId,
            Name = userRole.Name,
            Description = userRole.Description
        };
    }

    /// <summary>
    /// A helper function to create a user model from entity.
    /// </summary>
    /// <param name="user">An user entity to transform.</param>
    /// <returns>An User model object.</returns>
    public static Core.Models.User MapToUser(this Entities.User user)
    {
        return new Core.Models.User
        {
            UserGuid = user.UserId,
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
    public static Core.Models.ForumCategory MapToCategory(this Entities.ForumCategory category)
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
    public static Core.Models.ForumPost MapToForumPost(this Entities.ForumPost post)
    {
        return new Core.Models.ForumPost
        {
            ForumPostId = post.ForumPostId,
            Title = post.Title,
            Content = post.Content != null
                ? post.Content
                : "",
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            Category = post.ForumCategory.MapToCategory(),
            User = post.User.MapToUser()
        };
    }
}
