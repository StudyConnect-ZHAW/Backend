namespace StudyConnect.Core.Models;

public class GroupPost
{
    public Guid GroupPostId { get; set; }

    public required string Title { get; set; }

    public required string? Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid GroupId { get; set; }

    public User? User { get; set; }
}
