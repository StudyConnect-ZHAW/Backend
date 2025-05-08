namespace StudyConnect.Core.Models;

public class ForumComment
{
    public required string Content { get; set; }

    public Guid ForumcommentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int ReplyCount { get; set; }

    public bool IsEdited { get; set; }

    public bool isDeleted { get; set; }

    public ForumPost? Post { get; set; }

    public ForumComment? ParentComment { get; set; }

    public User? User { get; set; }

    public ICollection<ForumComment>? ChildComments { get; set; }
}
