using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Dtos.Requests.Forum;

/// <summary>
/// Data transfer object for updating an existing forumpost.
/// </summary>
public class PostUpdateDto
{
    /// <summary>
    /// The new title to replace the old.
    /// </summary>
    [StringLength(255)]
    public required string Title { get; set; }
    /// <summary>
    /// the new content to replace the old.
    /// </summary>
    [Required(ErrorMessage = "Content change is required.")]
    public required string Content { get; set; }
}
