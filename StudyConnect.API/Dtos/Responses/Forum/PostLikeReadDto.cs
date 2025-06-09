namespace StudyConnect.API.Dtos.Responses.Forum;

/// <summary>
/// Data transfer object for reading forum post like information.
/// </summary>
public class PostLikeReadDto
{
    /// <summary>
    /// The unique identifier of the like.
    /// </summary>
    public Guid LikeId { get; set; }

    /// <summary>
    /// The unique identifier of the current user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The unique identifier of the current forum post.
    /// </summary>
    public Guid? ForumPostId { get; set; }

    /// <summary>
    /// The date and time when the post was liked.
    /// </summary>
    public DateTime? LikedAt { get; set; }
}
