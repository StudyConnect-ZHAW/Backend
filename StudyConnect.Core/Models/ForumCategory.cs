using System;


namespace StudyConnect.Core.Models;

public class ForumCategory
{
    public Guid ForumCategoryId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
