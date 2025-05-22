using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities
{
    /// <summary>
    /// Represents a tag that can be assigned to posts or content.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets the unique identifier for the tag.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid TagId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the optional description of the tag.
        /// </summary>
        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        /// <summary>
        /// Future: Collection of related posts (optional navigation property).
        /// </summary>
        public virtual ICollection<PostTag>? PostTags { get; set; } = [];
    }
}
