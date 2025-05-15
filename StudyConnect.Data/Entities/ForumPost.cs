using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a post in the forum, which can contain comments and is associated with a specific category and user.
/// </summary>
public class ForumPost
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public Guid ForumPostId { get; set; }

    [Required]
    [MaxLength(200)]
    [DataType(DataType.Text)]
    public required string Title { get; set; }

    [Required]
    [DataType(DataType.MultilineText)]
    public string? Content { get; set; }

    [Required]
    public Guid ForumCategoryId { get; set; } 

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 

    public int ViewCount { get; set; } = 0;

    public int CommentCount { get; set; } = 0;

    /// <summary>
    /// Forum category this post belongs to.
    /// </summary>
    [ForeignKey("ForumCategoryId")]
    public virtual ForumCategory ForumCategory { get; set; } = null!;

    /// <summary>
    /// User who created the post.
    /// </summary>
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Collection of comments associated with this post.
    /// </summary>
    public virtual ICollection<ForumComment> ForumComments { get; set; } = [];
}
