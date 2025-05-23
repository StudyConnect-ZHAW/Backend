using System;

namespace StudyConnect.API.Dtos.Responses.Forum;

/// <summary>
/// Data transfer object for reading Category information.
/// </summary>
public class CategoryReadDto
{

    /// <summary>
    /// the unique identifier of the category
    /// </summary>
    public Guid? ForumCategoryId { get; set; }

    /// <summary>
    /// the name of the Category
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// the description of the Category 
    /// </summary>
    public string? Description { get; set; }
}
