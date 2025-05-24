using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Dtos.Requests.Group;

/// <summary>
/// Data transfer object for creating a new forumpost.
/// </summary>
public class GroupPostDto
{
    /// <summary>
    /// The title of the post.
    /// </summary>
    [Required(ErrorMessage = "GroupPost Title is required.")]
    [StringLength(200)]
    public required string Title { get; set; }

    /// <summary>
    /// The content of the post.
    /// </summary>
    public string? Content { get; set; }
}

