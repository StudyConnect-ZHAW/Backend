namespace StudyConnect.Core.Models
{
    /// <summary>
    /// Represents a tag that can be assigned to posts or content for categorization.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets the unique identifier for the tag.
        /// </summary>
        public Guid TagId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the optional description of the tag.
        /// </summary>
        public string? Description { get; set; }
    }
}
