using System;
using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Dtos.Requests.Forum;

/// <summary>
/// Data transfer object for creating a new category.
/// </summary>
public class CategoryCreateDto
{
    
    /// <summary>
    /// The Name of the category.
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(255)]
    public required string Name { get; set; }

    /// <summary>
    /// The description of the category.
    /// </summary>
    [StringLength(255)]
    public string? Description { get; set; }
}
