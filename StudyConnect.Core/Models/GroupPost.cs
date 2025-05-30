using System.ComponentModel.DataAnnotations;

namespace StudyConnect.Core.Models;

public class GroupPost
{
    public Guid GroupPostId { get; set; }

    [MaxLength(200)]
    public required string Title { get; set; }

    [MaxLength(500)]
    public required string? Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int CommentCount { get; set; }

    public GroupMember? GroupMember { get; set; }
}
