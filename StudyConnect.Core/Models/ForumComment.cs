using System.ComponentModel.DataAnnotations;

namespace StudyConnect.Core.Models;

public class ForumComment
{
    
    [MaxLength(500)]
    public required string Content { get; set; }

    public Guid ForumCommentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int ReplyCount { get; set; }

    public bool IsEdited { get; set; }

    public bool IsDeleted { get; set; }

    public Guid PostId { get; set; }

    public int LikeCount { get; set; }

    public Guid? ParentCommentId { get; set; }

    public User? User { get; set; }

    public ICollection<ForumComment>? Replies { get; set; }
}
