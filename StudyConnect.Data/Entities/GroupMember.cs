using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a member of a group in the system, including their role and status within the group.
/// The member can be a user with specific permissions and responsibilities within the group context.
/// </summary>
public class GroupMember
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public Guid GroupMemberId { get; set; }

    [Required]
    public Guid MemberId { get; set; }

    [Required]
    public Guid GroupId { get; set; }

    [Required]
    public Guid MemberRoleId { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Unique identifier for the group member.
    /// </summary>

    [ForeignKey("MemberId")]
    public User Member { get; set; } = null!;

    /// <summary>
    /// Unique identifier for the group to which the member belongs.
    /// </summary>
    [ForeignKey("GroupId")]
    public Group Group { get; set; } = null!;

    /// <summary>
    /// Role of the member within the group, such as admin, moderator, or member.
    /// </summary>
    [ForeignKey("MemberRoleId")]
    public MemberRole MemberRole { get; set; } = null!;

    public virtual ICollection<GroupPost> GroupPosts { get; set; } = [];

    public virtual ICollection<GroupComment> GroupComments { get; set; } = [];
}
