namespace StudyConnect.API.Dtos.Responses
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing a tag in the API response.
    /// </summary>
    public class TagDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the tag.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the optional description of the tag.
        /// </summary>
        public string ? Description { get; set; }
    }
}