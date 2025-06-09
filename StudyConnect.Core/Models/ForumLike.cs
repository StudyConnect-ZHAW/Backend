namespace StudyConnect.Core.Models;

public class ForumLike
{
    public Guid LikeId { get; set; }

    public Guid UserId { get; set; }

    public Guid? ForumPostId { get; set; }

    public Guid? ForumCommentId { get; set; }

    public DateTime LikedAt { get; set; }
}
