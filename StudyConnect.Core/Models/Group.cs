using System;
using System.ComponentModel.DataAnnotations;

namespace StudyConnect.Core.Models;

/// <summary>
/// Represents a group within the StudyConnect system.
/// </summary>
public class Group
{
    /// <summary>
    /// The unique identifier of the group.
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    /// The unique identifier of the user who owns the group.
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// The name of the group.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The Description of the group.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The CreatedAt of the group.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
