using System;

namespace StudyConnect.Core.Models;

public class UserRole
{
     public Guid URoleId { get; set; }

     public required string Name { get; set; }

     public string? Description { get; set; }
}
