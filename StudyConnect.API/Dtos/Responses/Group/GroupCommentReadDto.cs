using StudyConnect.API.Dtos.Responses.User;

namespace StudyConnect.API.Dtos.Responses.Group;

/// <summary>
/// Data transfer object for reading forum comment information.
/// </summary>
public class GroupCommentReadDto
{
    /// <summary>
    /// The unique identifier of the forum comment.
    /// </summary>
    public Guid? GroupCommentId { get; set; }

    /// <summary>
    /// The content of the forum comment.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// The date and time when comment was Created.
    /// </summary>
    public DateTime? Created { get; set; }

    /// <summary>
    /// The date and time when comment was Updated.
    /// </summary>
    public DateTime? Updated { get; set; }

    /// <summary>
    /// A state indicating that the comment was modified.
    /// </summary>
    public bool Edited { get; set; }

    /// <summary>
    /// The unique identifier of the post the comment belongs to.
    /// </summary>
    public Guid? GroupPostId { get; set; }

    /// <summary>
    /// The creator of this forum comment.
    /// </summary>
    public UserReadDto? Member { get; set; }
}
