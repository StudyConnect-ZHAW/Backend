namespace StudyConnect.Data.Utilities;

public static class ModelMappers
{
    /// <summary>
    /// A helper function to create a user model from entity.
    /// </summary>
    /// <param name="user">An user entity to transform.</param>
    /// <returns>An User model object.</returns>
    public static Core.Models.User ToUserModel(this Entities.User user)
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
    public static Core.Models.ForumCategory? ToCategoryModel(this Entities.ForumCategory? category)
    {
        if (category == null)
            return null;

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
    public static Core.Models.ForumPost? ToForumPostModel(this Entities.ForumPost? post)
    {
        if (post == null)
            return null;

        return new Core.Models.ForumPost
        {
            ForumPostId = post.ForumPostId,
            Title = post.Title,
            Content = post.Content != null
                ? post.Content
                : "",
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            CommentCount = post.CommentCount,
            Category = post.ForumCategory.ToCategoryModel(),
            User = post.User.ToUserModel()
        };
    }

    public static Core.Models.ForumComment ToCommentModel(this Entities.ForumComment comment)
    {
        return new Core.Models.ForumComment
        {
            Content = comment.IsDeleted
                ? ""
                : comment.Content,
            ForumCommentId = comment.ForumCommentId,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            ReplyCount = comment.ReplyCount,
            IsEdited = comment.IsEdited,
            IsDeleted = comment.IsDeleted,
            PostId = comment.ForumPostId,
            ParentCommentId = comment.ParentComment != null
                ? comment.ParentComment.ForumCommentId
                : null,
            User = comment.IsDeleted
                ? null
                : comment.User.ToUserModel(),
            Replies = comment.Replies != null
                ? comment.Replies.Select(c => c.ToCommentModel()).ToList()
                : null
        };
    }

    public static Core.Models.ForumLike? ToLikeModel(this Entities.ForumLike like)
    {
        if (like == null)
            return null;

        return new Core.Models.ForumLike
        {
            ForumLikeId = like.LikeId,
            LikedAt = like.LikedAt,
            User = like.User.ToUserModel(),
            Post = like.ForumPost != null
                ? like.ForumPost.ToForumPostModel()
                : null,
            Comment = like.ForumComment != null
                ? like.ForumComment.ToCommentModel()
                : null
        };
    }
}
