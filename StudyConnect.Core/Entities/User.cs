using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Core.Entities;

/// <summary>
/// Represents a user in the StudyConnect application.
/// </summary>
public class User
{
    /// <summary>
    /// Unique identifier for the user from microsoft identity.
    /// </summary>
    [Key]
    public Guid UserGuid { get; set; }

    /// <summary>
    /// Foreign key to the UserRole entity.
    /// </summary>
    [Required]
    public Guid URole_ID { get; set; }

    /// <summary>
    /// Navigation property for the user's role.
    /// </summary>
    [ForeignKey("URoleId")]
    public virtual UserRole? Role { get; set; }

    /// <summary>
    /// First name of the user.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public required string FirstName { get; set; }

    /// <summary>
    /// Last name of the user
    /// </summary>
    [Required]
    [MaxLength(255)]
    public required string LastName { get; set; }

    /// <summary>
    /// Email address of the user from microsoft identity.
    /// This property is required and must be a valid email format.
    /// The maximum length of the email address is 255 characters.
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public required string Email { get; set; }
}
