using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a member of a group, including their role and status within the group.
/// </summary>
public class GroupMembers
{
    [Key]
    public Guid GroupMemberId { get; set; }

    [Required]
    public Guid GroupId { get; set; }

    [ForeignKey("GroupId")]
    public virtual required Group Group { get; set; }

    [Required]
    public Guid UserGuid { get; set; }

    [ForeignKey("UserGuid")]
    public virtual required User User { get; set; }

    [Required]
    public Guid MemberRoleId { get; set; }

    [ForeignKey("MemberRoleId")]
    public virtual required MemberRole MemberRole { get; set; }

    [Required]
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public Boolean IsActive { get; set; } = true;
}
