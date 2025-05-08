using System;

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
    /// The ID of the user who owns the group.
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// The name of the group.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of the group.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
