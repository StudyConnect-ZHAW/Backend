using System;
using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Dtos.Requests.Group;

/// <summary>
/// Data transfer object for updating the name and description of an existing group.
/// </summary>
public class GroupUpdateDto
{
    /// <summary>
    /// The updated name of the group. Maximum length is 255 characters.
    /// </summary>
    [Required(ErrorMessage = "Group name is required.")]
    [StringLength(255)]
    public required string Name { get; set; }

    /// <summary>
    /// The updated description of the group. Maximum length is 1000 characters.
    /// </summary>
    [Required(ErrorMessage = "Group description is required.")]
    [StringLength(1000)]
    public required string Description { get; set; }
}
