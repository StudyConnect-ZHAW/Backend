using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Dtos.Requests.Forum;
/// <summary>
/// Data transfer object for leaving a like.
/// </summary>
public class LikeCreateDto
{
    /// <summary>
    /// The unique identifier of the current user.
    /// </summary>
    [Required(ErrorMessage = "User Id is Required.")]
    public required Guid User { get; set; }

    /// <summary>
    /// The unique identifier of the post to like (optional).
    /// </summary>
    public Guid? PostId { get; set; }

    /// <summary>
    /// The unique identifier of the commment to like (optional).
    /// </summary>
    public Guid? CommentId { get; set; }
      
}
