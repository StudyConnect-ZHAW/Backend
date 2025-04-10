using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a post in the forum, which can contain discussions and comments from users.
/// </summary>
public class ForumPost
{

    [Key]
    public Guid ForumPostId { get; set; }

    [Required]
    public Guid ForumCategoryId { get; set; }

    [ForeignKey("ForumCategoryId")]
    public virtual ForumCategory? ForumCategory { get; set; }

    [Required]
    public Guid UserGuid { get; set; }

    [ForeignKey("UserGuid")]
    public virtual User? User { get; set; }

    [Required]
    [MaxLength(255)]
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
}
