using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a group in the system, which can have multiple members and categories.
/// The group is owned by a user and can have various properties such as name, description, and visibility.
/// </summary>
public class Group
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public Guid GroupId { get; set; }

    [Required]
    public Guid OwnerId { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Description { get; set; }

    public bool Visibility { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Owner of the group.
    /// </summary>
    [ForeignKey("OwnerId")]
    public required virtual User Owner { get; set; }

    /// <summary>
    /// Collection of members in the group.
    /// </summary>
    public virtual ICollection<GroupMember> GroupMembers { get; set; } = [];








}
