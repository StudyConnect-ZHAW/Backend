using System.ComponentModel.DataAnnotations;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a role that a member can have within the system, such as admin, lecturer, or student.
/// </summary>
public class UserRole
{
    /// <summary>
    /// Unique identifier for the user role.
    /// </summary>
    [Key]
    public Guid URoleId { get; set; }

    /// <summary>
    /// Name of the role.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }

    /// <summary>
    /// Description of the role.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public required string Description { get; set; }

    /// <summary>
    /// Collection of users with this role.
    /// </summary>
    public virtual ICollection<User>? Users { get; set; }
}
