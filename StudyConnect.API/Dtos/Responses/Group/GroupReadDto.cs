using System;
using StudyConnect.API.Dtos.Responses.User;

namespace StudyConnect.API.Dtos.Responses.Group;

/// <summary>
/// Data transfer object for reading group information from the system.
/// </summary>
public class GroupReadDto
{
    /// <summary>
    /// The unique identifier of the group.
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    /// The name of the group.
    /// </summary>
    public string? Name { get; set; } 

    /// <summary>
    /// The description of the group.
    /// </summary>
    public string? Description { get; set; } 

    /// <summary>
    /// Timestamp when the group was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The owner of the group.
    /// </summary>
    public UserReadDto? Owner { get; set; }
}
