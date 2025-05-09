using StudyConnect.API.Dtos.Responses.User;

namespace StudyConnect.API.Dtos.Responses.Forum;

/// <summary>
/// Data transfer object for reading forum comment information.
/// </summary>
public class CommentReadDto
{
    /// <summary>
    /// The unique identifier of the forum comment.
    /// </summary>
    public Guid? ForumCommentId { get; set; }

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
    /// The count of the replies to this comment.
    /// </summary>
    public int? ReplyCount { get; set; }

    /// <summary>
    /// A state indicating that the comment was modified.
    /// </summary>
    public bool Edited { get; set; }

    /// <summary>
    /// A state indicating that the comment should not be accessed anymore.
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// The unique identifier of the post the comment belongs to.
    /// </summary>
    public Guid? PostId { get; set; }

    /// <summary>
    /// The parent comment of this comment, in case this comment is a reply.
    /// </summary>
    public Guid? ParentCommentId { get; set; }

    /// <summary>
    /// The creator of this forum comment.
    /// </summary>
    public UserReadDto? User { get; set; }

    /// <summary>
    /// A list of replies to this comment as dtos.
    /// </summary>
    public ICollection<CommentReadDto>? Replies { get; set; }
}
