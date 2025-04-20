using System;


namespace StudyConnect.Core.Models;

public class ForumCategory
{
    public Guid ForumCatergoryId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
