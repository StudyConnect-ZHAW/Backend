using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a comment made by a user on a forum post.
/// This class includes properties for the comment's content, creation and update timestamps,
/// like/dislike counts, reply count, and various flags for comment status (pinned, edited, deleted).
/// It also includes navigation properties to the user who made the comment and the forum post it belongs to.
/// The class supports hierarchical comments, allowing for replies to other comments.
/// </summary>
public class ForumComment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public Guid ForumCommentId { get; set; }

    [Required]
    [MaxLength(500)]
    public required string Content { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid ForumPostId { get; set; }

    public Guid? ParentCommentId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int ReplyCount { get; set; } = 0;

    public bool IsEdited { get; set; } = false;

    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// User who created the comment.
    /// </summary>
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    /// <summary>
    /// Forum post this comment belongs to.
    /// </summary>
    [ForeignKey("ForumPostId")]
    public ForumPost ForumPost { get; set; } = null!;

    /// <summary>
    /// Parent comment this comment is replying to.
    /// This property is nullable to allow for top-level comments th
    /// </summary>
    [ForeignKey("ParentCommentId")]
    public ForumComment? ParentComment { get; set; } = null!;

    /// <summary>
    /// Collection of replies to this comment.
    /// This property is initialized to an empty list to avoid null reference exceptions.
    /// </summary>
    public virtual ICollection<ForumComment> Replies { get; set; } = new List<ForumComment>();


    /// <summary>
    /// Collection of likes associated with this comment.
    /// </summary>
    public virtual ICollection<ForumLike> ForumLikes { get; set; } = [];
}
