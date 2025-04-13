using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a user in the system, including their personal information and roles.
/// The user can own groups, participate in discussions, and have various roles within the system.
/// The user is identified by a unique GUID from his Microsoft Identity.
/// </summary>
public class User
{
    [Key]
    [Required]
    public Guid UserGuid { get; set; }

    [Required]
    public Guid URoleId { get; set; }

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    /// <summary>
    /// User role in the system.
    /// </summary>
    [ForeignKey("URoleId")]
    public required UserRole URole { get; set; }

    /// <summary>
    /// Collection of groups owned by the user.
    /// </summary>
    public virtual ICollection<Group> Groups { get; set; } = [];

    /// <summary>
    /// Collection of groups the user is a member of.
    /// </summary>
    public virtual ICollection<GroupMembers> GroupMembers { get; set; } = [];

    /// <summary>
    /// Collection of forum posts made by the user.
    /// </summary>
    public virtual ICollection<ForumPost> ForumPosts { get; set; } = [];

    /// <summary>
    /// Collection of forum comments made by the user.
    /// </summary>
    public virtual ICollection<ForumComment> ForumComments { get; set; } = [];
}
