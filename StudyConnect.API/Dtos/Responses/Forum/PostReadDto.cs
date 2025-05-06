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
    public CategoryReadDto? Modul { get; set; }

    /// <summary>
    /// The creator of the post as a dto.
    /// </summary>
    public UserReadDto? Author { get; set; }
}

