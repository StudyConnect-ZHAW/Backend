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
    public required string Title {get; set;}
    
    /// <summary>
    /// The category name the post should be assigned to.
    /// </summary>
    [Required(ErrorMessage = "Catergory Name is required.")]
    [StringLength(255)]
    public required string Modul {get;  set; }

    /// <summary>
    /// The content of the post.
    /// </summary>
    public string? Content {get; set; }
}

