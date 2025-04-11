using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a post in the forum, which can contain comments and is associated with a specific category and user.
/// </summary>
public class ForumPost
{
    [Key]
    public Guid ForumPostId { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public string? Content { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public Boolean IsActive { get; set; } = true;

    [Required]
    public Boolean IsPinned { get; set; } = false;

    [Required]
    public Boolean IsLocked { get; set; } = false;

    [Required]
    public int ViewCount { get; set; } = 0;

    [Required]
    public int CommentCount { get; set; } = 0;

    [Required]
    public int LikeCount { get; set; } = 0;

    [Required]
    public int DislikeCount { get; set; } = 0;

    /// <summary>
    /// Forum category this post belongs to.
    /// </summary>
    public required ForumCategory ForumCategory { get; set; }

    /// <summary>
    /// User who created the post.
    /// </summary>
    public required User User { get; set; }

    /// <summary>
    /// Collection of comments associated with this post.
    /// </summary>
    public virtual ICollection<ForumComment> ForumComments { get; set; } = new List<ForumComment>();
}
