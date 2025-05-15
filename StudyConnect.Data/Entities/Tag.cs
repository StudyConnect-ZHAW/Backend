using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyConnect.Data.Entities
{
    /// <summary>
    /// Represents a tag that can be assigned to posts or content.
    /// </summary>
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid TagId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        /// <summary>
        /// Future: Collection of related posts (optional navigation property).
        /// </summary>
    }
}
