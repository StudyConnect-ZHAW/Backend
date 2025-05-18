using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Dtos.Requests.Forum;
/// <summary>
/// Data transfer object for leaving a like.
/// </summary>
public class LikeCommentCreateDto
{
    /// <summary>
    /// The unique identifier of the current user.
    /// </summary>
    [Required(ErrorMessage = "User Id is Required.")]
    public required Guid UserId { get; set; }

    /// <summary>
    /// The unique identifier of the comment to like.
    /// </summary>
    public Guid? CommentId { get; set; }

    /// <summary>
    /// The unique identifier of the post to like.
    /// </summary>
    public Guid? PostId { get; set; }
}
