using StudyConnect.Data.Entities;

namespace StudyConnect.Data;

public static class ModelMapper
{
    /// <summary>
    /// A helper function to map a user entity to its model representation.
    /// </summary>
    /// <param name="user">A user entity to transform.</param>
    /// <returns>A User model object.</returns>
    public static Core.Models.User ToUserModel(this User user)
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
    /// A helper function to map a category entity to its model representation.
    /// </summary>
    /// <param name="category">A category entity to transform.</param>
    /// <returns>A forumCategory model object.</returns>
    public static Core.Models.ForumCategory ToCategoryModel(this ForumCategory category)
    {
        return new Core.Models.ForumCategory
        {
            ForumCategoryId = category.ForumCategoryId,
            Name = category.Name,
            Description = category.Description
        };
    }

    /// <summary>
    /// A helper function to map a group entity to its model representation.
    /// </summary>
    /// <param name="category">A group entity to transform.</param>
    /// <returns>A forumCategory model object.</returns>
    public static Core.Models.Group ToGroupModel(this Group group)
    {
        return new()
        {
            GroupId = group.GroupId,
            OwnerId = group.OwnerId,
            Name = group.Name,
            Description = group.Description,
            CreatedAt = group.CreatedAt
        };
    }

}
