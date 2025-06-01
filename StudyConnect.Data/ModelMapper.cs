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
        return new()
        {
            UserGuid = user.UserGuid,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
        };
    }

    /// <summary>
    /// A helper function to map a category entity to its model representation.
    /// </summary>
    /// <param name="category">A category entity to transform.</param>
    /// <returns>A forumCategory model object.</returns>
    public static Core.Models.ForumCategory ToCategoryModel(this ForumCategory category)
    {
        return new()
        {
            ForumCategoryId = category.ForumCategoryId,
            Name = category.Name,
            Description = category.Description,
        };
    }

    /// <summary>
    /// A helper function to map a group entity to its model representation.
    /// </summary>
    /// <param name="group">A group entity to transform.</param>
    /// <returns>A Group model object.</returns>
    public static Core.Models.Group ToGroupModel(this Group group)
    {
        return new()
        {
            GroupId = group.GroupId,
            OwnerId = group.OwnerId,
            Name = group.Name,
            Description = group.Description,
            CreatedAt = group.CreatedAt.ToUniversalTime(),
            Owner = group.Owner.ToUserModel(),
        };
    }

    /// <summary>
    /// A helper function to map a member entity to its model representation.
    /// </summary>
    /// <param name="member">A group member entity to transform.</param>
    /// <returns>A GroupMember model object.</returns>
    public static Core.Models.GroupMember ToMemberModel(this GroupMember member)
    {
        return new()
        {
            MemberId = member.MemberId,
            GroupId = member.GroupId,
            JoinedAt = member.JoinedAt.ToUniversalTime(),
            Member = member.Member.ToUserModel(),
        };
    }

    /// <summary>
    /// A helper function to map a forum like entity to its model representation.
    /// </summary>
    /// <returns>A GroupMember model object.</returns>
    public static Core.Models.ForumLike ToForumLikeModel(this ForumLike like)
    {
        return new()
        {
            LikeId = like.LikeId,
            UserId = like.UserId,
            ForumPostId = like.ForumPostId ?? Guid.Empty,
            ForumCommentId = like.ForumCommentId ?? Guid.Empty,
        };
    }
}
