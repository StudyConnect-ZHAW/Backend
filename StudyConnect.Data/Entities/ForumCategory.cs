using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities;

/// <summary>
/// Represents a category in the forum, which can contain multiple posts.
/// </summary>
public class ForumCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public Guid ForumCategoryId { get; set; }

    [Required]
    public required string Name { get; set; }

    [MaxLength(500)]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    /// <summary>
    /// Collection of posts associated with this category.
    /// </summary>
    public ICollection<ForumPost> ForumPosts { get; set; } = new List<ForumPost>();
}
