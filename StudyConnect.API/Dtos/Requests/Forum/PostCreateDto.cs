using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Dtos.Requests.Forum;

/// <summary>
/// Data transfer object for creating a new forumpost.
/// </summary>
public class PostCreateDto
{
    /// <summary>
    /// The title of the post.
    /// </summary>
    [Required(ErrorMessage = "Post Title is required.")]
    [StringLength(255)]
    public required string Title { get; set; }

    /// <summary>
    /// The unique identifier of the category the post should be assigned to.
    /// </summary>
    [Required(ErrorMessage = "Forum Category ID is required.")]
    public required Guid ForumCategoryId { get; set; }

    /// <summary>
    /// The content of the post.
    /// </summary>
    public string? Content { get; set; }
}

