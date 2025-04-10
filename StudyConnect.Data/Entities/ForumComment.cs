using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a comment in the forum, which can be a reply to a post or another comment.
/// </summary>
public class ForumComment
{
    [Key]
    public Guid ForumCommentId { get; set; }

    [Required]
    public Guid ForumPostId { get; set; }

    [ForeignKey("ForumPostId")]
    public virtual ForumPost? ForumPost { get; set; }

    [Required]
    public Guid UserGuid { get; set; }

    [ForeignKey("UserGuid")]
    public virtual User? User { get; set; }

    public Guid ParentCommentId { get; set; } = Guid.Empty;

    [Required]
    public string? Content { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public int LikeCount { get; set; } = 0;

    [Required]
    public int DislikeCount { get; set; } = 0;

    [Required]
    public int ReplyCount { get; set; } = 0;

    [Required]
    public Boolean IsPinned { get; set; } = false;

    [Required]
    public int ViewCount { get; set; } = 0;

    [Required]
    public Boolean IsEdited { get; set; } = false;

    [Required]
    public Boolean IsDeleted { get; set; } = false;
}
