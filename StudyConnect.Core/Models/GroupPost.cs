namespace StudyConnect.Core.Models;

public class GroupPost
{
    public Guid GroupPostId { get; set; }

    public required string Title { get; set; }

    public required string? Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int CommentCount { get; set; }

    public GroupMember? GroupMember { get; set; }
}
