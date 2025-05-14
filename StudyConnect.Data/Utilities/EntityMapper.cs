namespace StudyConnect.Data.Utilities;

public static class EntityMapper
{
    public static Data.Entities.User MapFromUser(this Core.Models.User user)
    {
        return new Data.Entities.User
        {
            UserGuid = user.UserGuid,
            URole = user.userRole!.MapFromURole(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public static Data.Entities.UserRole MapFromURole(this Core.Models.UserRole uRole)
    {
        return new Data.Entities.UserRole
        {
            URoleId = uRole.URoleId,
            Name = uRole.Name,
            Description = uRole.Description
        };
    }

    public static Data.Entities.ForumCategory MapFromCategory(this Core.Models.ForumCategory category)
    {
        return new Data.Entities.ForumCategory
        {
            Name = category.Name,
            Description = category.Description
        };
    }

    public static Data.Entities.ForumPost MapFromForumPost(this Core.Models.ForumPost post, Core.Models.ForumCategory? category, Core.Models.User? user)
    {
        return new Data.Entities.ForumPost
        {
            Title = post.Title,
            Content = post.Content,
            UpdatedAt = DateTime.UtcNow,
            ForumCategory = category != null
                ? category.MapFromCategory()
                : post.Category!.MapFromCategory(),
            User = user != null
                ? user.MapFromUser()
                : post.User!.MapFromUser(),
        };
    }

}
