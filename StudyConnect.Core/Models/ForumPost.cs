using System.ComponentModel.DataAnnotations;

namespace StudyConnect.Core.Models;

public class ForumPost
{

    [MaxLength(200)]
    public required string Title { get; set; }

    [MaxLength(500)]
    public required string? Content { get; set; }

    public Guid ForumPostId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int LikeCount { get; set; }

    public int CommentCount { get; set; }

    public ForumCategory? Category { get; set; }

    public User? User { get; set; }
}
