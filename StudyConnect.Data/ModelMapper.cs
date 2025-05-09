namespace StudyConnect.Data;

public static class ModelMapper
{
    /// <summary>
    /// A helper function to create a user model from entity.
    /// </summary>
    /// <param name="user">An user entity to transform.</param>
    /// <returns>An User model object.</returns>
    public static Core.Models.User MapUserToModel(Entities.User user)
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
    public static Core.Models.ForumCategory MapCategoryToModel(Entities.ForumCategory category)
    {
        return new Core.Models.ForumCategory
        {
            ForumCategoryId = category.ForumCategoryId,
            Name = category.Name,
            Description = category.Description
        };
    }

}
