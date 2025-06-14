using System.ComponentModel.DataAnnotations;

namespace StudyConnect.Core.Models;

public class GroupComment
{
    [MaxLength(500)]
    public required string Content { get; set; }

    public Guid GroupCommentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsEdited { get; set; }

    public Guid GroupPostId { get; set; }

    public GroupMember? GroupMember { get; set; }
}
