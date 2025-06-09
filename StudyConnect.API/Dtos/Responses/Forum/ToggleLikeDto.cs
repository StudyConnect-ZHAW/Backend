namespace StudyConnect.API.Dtos.Responses.Forum;

/// <summary>
/// Data transfer object for reading toggling like information.
/// </summary>
public class ToggleLikeDto
{
    /// <summary>
    /// A boolean to show if a post/comment is liked.
    /// </summary>
    public bool AddedLike { get; set; }
}
