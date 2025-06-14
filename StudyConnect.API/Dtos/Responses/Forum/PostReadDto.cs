using StudyConnect.API.Dtos.Responses.User;

namespace StudyConnect.API.Dtos.Responses.Forum;

/// <summary>
/// Data transfer object for reading Post information.
/// </summary>
public class PostReadDto
{
    /// <summary>
    /// The unique identifier of the post.
    /// </summary>
    public Guid? ForumPostId { get; set; }

    /// <summary>
    /// The title of the post, given by the author.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The content of the post.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// The number of comments the post has.
    /// </summary>
    public int? CommentCount { get; set; }

    /// <summary>
    /// The number of likes the post has received.
    /// </summary>
    public int? LikeCount { get; set; }

    /// <summary>
    /// Indicates whether the currently authenticated user has liked this post.
    /// </summary>
    public bool LikedByCurrentUser { get; set; }

    /// <summary>
    /// The date and time when post was created.
    /// </summary>
    public DateTime? Created { get; set; }

    /// <summary>
    /// The date and time when the post was updated.
    /// </summary>
    public DateTime? Updated { get; set; }

    /// <summary>
    /// The category of the post as a dto.
    /// </summary>
    public CategoryReadDto? Category { get; set; }

    /// <summary>
    /// The creator of the post as a dto.
    /// </summary>
    public UserReadDto? User { get; set; }
}
