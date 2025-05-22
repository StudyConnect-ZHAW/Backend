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
    public required string Content { get; set; } 
}
