using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

public class MemberRole
{
    /// <summary>
    /// Unique identifier for the group role.
    /// </summary>
    [Key]
    public Guid MemberRoleId { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Description { get; set; }
}
