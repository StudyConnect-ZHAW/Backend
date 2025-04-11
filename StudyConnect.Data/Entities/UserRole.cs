using System.ComponentModel.DataAnnotations;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a role that a user can have in the system, such as admin, lecturer, or student.
/// The role defines the permissions and responsibilities of the user within the system.
/// </summary>
public class UserRole
{
    [Key]
    public Guid URoleId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    /// <summary>
    /// Collection of users with this role.
    /// </summary>
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
