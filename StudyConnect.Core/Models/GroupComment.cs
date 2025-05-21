namespace StudyConnect.Core.Models;

public class GroupComment
{
    public required string Content { get; set; }

    public Guid ForumCommentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsEdited { get; set; }

    public bool IsDeleted { get; set; }

    public Guid GroupPostId { get; set; }

    public Group? Group { get; set; }

    public User? User { get; set; }

}
