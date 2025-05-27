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
    /// The name of the group.
    /// </summary>
    public string? Name { get; set; } 

    /// <summary>
    /// The description of the group.
    /// </summary>
    public string? Description { get; set; } 

    /// <summary>
    /// The ID of the user who owns the group.
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// The first name of the group owner.
    /// </summary>
    public string? OwnerFirstName { get; set; } 

    /// <summary>
    /// The last name of the group owner.
    /// </summary>
    public string? OwnerLastName { get; set; }

    /// <summary>
    /// The Email of the group owner.
    /// </summary>
    public string? OwnerEmail { get; set; }


    /// <summary>
    /// Timestamp when the group was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
