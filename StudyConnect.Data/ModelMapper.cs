using StudyConnect.Data.Entities;

namespace StudyConnect.Data;

public static class ModelMapper
{
    /// <summary>
    /// A helper function to create a user model from entity.
    /// </summary>
    /// <param name="user">A user entity to transform.</param>
    /// <returns>A User model object.</returns>
    public static Core.Models.User MapUserToModel(User user)
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
    /// <returns>A forumCategory model object.</returns>
    public static Core.Models.ForumCategory MapCategoryToModel(ForumCategory category)
    {
        return new Core.Models.ForumCategory
        {
            ForumCategoryId = category.ForumCategoryId,
            Name = category.Name,
            Description = category.Description
        };
    }

}
