using System.ComponentModel.DataAnnotations;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a category in the forum, which can contain multiple posts and discussions.
/// </summary>
public class ForumCategory
{
    [Key]
    public Guid ForumCategoryId { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Description { get; set; }
}
