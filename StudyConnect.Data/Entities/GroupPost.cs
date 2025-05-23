using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a post in the forum, which can contain comments and is associated with a specific category and user.
/// </summary>
public class GroupPost
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public Guid GroupPostId { get; set; }

    [Required]
    [MaxLength(200)]
    [DataType(DataType.Text)]
    public required string Title { get; set; }

    [Required]
    [MaxLength(500)]
    [DataType(DataType.MultilineText)]
    public string? Content { get; set; }

    [Required]
    public Guid GroupId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int CommentCount { get; set; } = 0;

    public Group Group { get; set; } = null!;

    /// <summary>
    /// Member: who created the post.
    /// </summary>
    public GroupMember GroupMember { get; set; } = null!;

    /// <summary>
    /// Collection of comments associated with this post.
    /// </summary>
    public virtual ICollection<GroupComment> GroupComments { get; set; } = [];
}
