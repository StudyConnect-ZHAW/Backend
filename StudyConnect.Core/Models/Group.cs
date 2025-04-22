using System;

namespace StudyConnect.Core.Models;

public class Group
{
        public Guid GroupId { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
}
