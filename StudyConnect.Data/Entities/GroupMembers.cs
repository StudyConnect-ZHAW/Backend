using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a member of a group in the system, including their role and status within the group.
/// The member can be a user with specific permissions and responsibilities within the group context.
/// </summary>
public class GroupMembers
{
    [Required]
    public Guid MemberId { get; set; }

    [Required]
    public Guid GroupId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid MemberRoleId { get; set; } = Guid.NewGuid();

    [Required]
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public Boolean IsActive { get; set; } = true;

    /// <summary>
    /// Unique identifier for the group member.
    /// </summary>
    [ForeignKey("MemberId")]
    public required User Member { get; set; }

    /// <summary>
    /// Unique identifier for the group to which the member belongs.
    /// </summary>
    [ForeignKey("GroupId")]
    public required Group Group { get; set; }

    /// <summary>
    /// Role of the member within the group, such as admin, moderator, or member.
    /// </summary>
    [ForeignKey("MemberRoleId")]
    public required MemberRole MemberRole { get; set; }
}
