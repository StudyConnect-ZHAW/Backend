using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Dtos.Requests.Forum;

/// <summary>
/// Data transfer object for updating a forum comment.
/// </summary>
public class CommentUpdateDto
{
    /// <summary>
    /// The Content of the forum comment.
    /// </summary>
    [Required(ErrorMessage = "Content is required.")]
    [StringLength(500)]
    public required string Content { get; set; }
}
