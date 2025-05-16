namespace StudyConnect.Data.Utilities;

public static class EntityMapper
{
    public static Data.Entities.User MapFromUser(this Core.Models.User user)
    {
        return new Data.Entities.User
        {
            UserId = user.UserGuid,
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

    public static Data.Entities.ForumComment MapCommentToEntity(this Core.Models.ForumComment comment, Guid userId, Guid postId, Guid? parrentId)
    {
        return new Data.Entities.ForumComment
        {
            Content = comment.Content,
            UserId = userId,
            ForumPostId = postId,
            ParentCommentId = parrentId != null
                ? parrentId
                : null
        };
    }
}
