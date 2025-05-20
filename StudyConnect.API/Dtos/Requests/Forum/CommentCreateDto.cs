using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Dtos.Requests.Forum;

/// <summary>
/// Data transfer object for creating a new forum comment.
/// </summary>
public class CommentCreateDto
{
    /// <summary>
    /// The unique identifier of the parent comment.
    /// </summary>
    public Guid? ParentCommentId { get; set; }

    /// <summary>
    /// The Content of the forum comment.
    /// </summary>
    [Required(ErrorMessage = "Content is required.")]
    public required string Content { get; set; }
}
