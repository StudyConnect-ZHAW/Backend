namespace StudyConnect.Core.Models;

public class GroupPost
{
    public required string Title { get; set; }

    public required string? Content { get; set; }

    public Guid GroupPostId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Group? Group { get; set; }

    public Guid? MemberId { get; set; }
}
