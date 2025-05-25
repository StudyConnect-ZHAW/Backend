using System;
using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Dtos.Requests.Group;

/// <summary>
/// Data transfer object for creating a new group.
/// </summary>
public class GroupCreateDto
{
    /// <summary>
    /// The name of the group.
    /// </summary>
    [Required(ErrorMessage = "Group name is required.")]
    [StringLength(255)]
    public required string Name { get; set; }

    /// <summary>
    /// The description of the group.
    /// </summary>
    [Required(ErrorMessage = "Group description is required.")]
    [StringLength(1000)]
    public required string Description { get; set; }
}
