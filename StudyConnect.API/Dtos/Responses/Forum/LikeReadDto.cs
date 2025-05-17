namespace StudyConnect.API.Dtos.Responses.Forum;

/// <summary>
/// Data transfer object for reading like count.
/// </summary>
public class LikeReadDto
{
    /// <summary>
    /// The like count of a specific post.
    /// </summary>
    public int? PostLikeCount { get; set; }

    /// <summary>
    /// The like count of a specific comment:
    /// </summary>
    public int? CommentLikeCount { get; set;}
}
