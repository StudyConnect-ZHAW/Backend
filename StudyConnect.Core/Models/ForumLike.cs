namespace StudyConnect.Core.Models;

public class ForumLike
{
    public Guid ForumLikeId { get; set; }

    public required User User { get; set; }

    public DateTime LikedAt { get; set; }

    public ForumPost? Post { get; set; }

    public ForumComment? Comment { get; set; }
}
