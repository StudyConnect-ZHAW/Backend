using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Dtos.Requests.Forum;

/// <summary>
/// Data transfer object for creating a new forumpost.
/// </summary>
public class PostCreateDto
{
    /// <summary>
    /// The unique identifier of the creator.
    /// </summary>
    [Required(ErrorMessage = "User Id is required.")]
    public required Guid UserId { get; set; }

    /// <summary>
    /// The title of the post.
    /// </summary>
    [Required(ErrorMessage = "Post Title is required.")]
    [StringLength(255)]
    public required string Title { get; set;}
    
    /// <summary>
    /// The category name the post should be assigned to.
    /// </summary>
    [Required(ErrorMessage = "User ID is required.")]
    public required Guid ForumCategoryId { get;  set; }

    /// <summary>
    /// The coModulID.
    /// </summary>
    public string? Content { get; set; }
}

