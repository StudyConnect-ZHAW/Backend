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
public class GroupComment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public Guid GroupCommentId { get; set; }

    [Required]
    public required string Content { get; set; }

    [Required]
    public Guid GroupPostId { get; set; }

    [Required]
    public Guid GroupMemberId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsEdited { get; set; } = false;

    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// User who created the comment.
    /// </summary>
    public GroupMember GroupMember { get; set; } = null!;

    /// <summary>
    /// Forum post this comment belongs to.
    /// </summary>
    public GroupPost GroupPost { get; set; } = null!;
}
