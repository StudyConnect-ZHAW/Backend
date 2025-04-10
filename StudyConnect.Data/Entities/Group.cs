using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

public class Group
{

    /// <summary>
    /// Unique identifier for the group.
    /// This property is the primary key and is generated using a GUID.
    /// </summary>
    [Key]
    public Guid GroupId { get; set; }


    /// <summary>
    /// Foreign key to the User entity.
    /// This property is required and represents the user who owns the group.
    /// </summary>
    [Required]
    public Guid UserGuid { get; set; }

    [ForeignKey("UserGuid")]
    public virtual User? User { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Description { get; set; }

    [Required]
    public Boolean Visibility { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;






}
