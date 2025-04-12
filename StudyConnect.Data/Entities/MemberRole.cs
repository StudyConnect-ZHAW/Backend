using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a role that a member can have within a group, such as admin, moderator, or member.
/// The role defines the permissions and responsibilities of the member within the group context.
/// </summary>
public class MemberRole
{
    /// <summary>
    /// Unique identifier for the group role.
    /// </summary>
    [Key]
    public Guid MemberRoleId { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Description { get; set; }

    /// <summary>
    /// Collection of group members associated with this role.
    /// </summary>
    public virtual ICollection<GroupMembers> GroupMembers { get; set; } = [];
}
