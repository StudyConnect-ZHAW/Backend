using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Dtos.Requests.Group;

/// <summary>
/// Data transfer object for creating a new forum comment.
/// </summary>
public class GroupCommentDto
{
    /// <summary>
    /// The Content of the forum comment.
    /// </summary>
    [Required(ErrorMessage = "Content is required.")]
    [StringLength(500)]
    public required string Content { get; set; }
}
