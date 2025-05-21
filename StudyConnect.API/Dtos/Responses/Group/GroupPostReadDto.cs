using StudyConnect.API.Dtos.Responses.User;

namespace StudyConnect.API.Dtos.Responses.Group;

/// <summary>
/// Data transfer object for reading Post information.
/// </summary>
public class GroupPostReadDto
{
    /// <summary>
    /// The unique identifier of the post.
    /// </summary>
    public Guid? GroupPostId { get; set; }

    /// <summary>
    /// The title of the post, given by the author.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The content of the post.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// The date and time when post was created. 
    /// </summary>
    public DateTime? Created { get; set; }

    /// <summary>
    /// The date and time when the post was updated.
    /// </summary>
    public DateTime? Updated { get; set; }

    /// <summary>
    /// The unique identifier of the group the post belongs to.
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    /// The creator of the post as a dto.
    /// </summary>
    public UserReadDto? User { get; set; }
}

