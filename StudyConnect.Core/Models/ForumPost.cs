

namespace StudyConnect.Core.Models;

public class ForumPost
{
    public required string Title { get; set; }
    public required string? Content { get; set; }
    public ForumCategory? Category { get; set; }
    public User? User { get; set; }
}
