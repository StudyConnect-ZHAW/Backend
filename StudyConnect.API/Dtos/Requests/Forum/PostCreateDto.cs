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
    [StringLength(200)]
    public required string Title { get; set; }

    /// <summary>
    /// The unique identifier of the category the post should be assigned to.
    /// </summary>
    [Required(ErrorMessage = "Forum Category ID is required.")]
    public required Guid ForumCategoryId { get; set; }

    /// <summary>
    /// The content of the post.
    /// </summary>
    [StringLength(500)]
    public string? Content { get; set; }
}

