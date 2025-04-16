using System;
using System.ComponentModel.DataAnnotations;

namespace StudyConnect.API.Dtos.Requests.User;

/// <summary>
/// Data transfer object for creating a new user.
/// </summary>
public class UserCreateDto
{   
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    [Required(ErrorMessage = "User GUID is required.")]
    [MinLength(36, ErrorMessage = "User GUID is to short.")]
    [StringLength(36, ErrorMessage = "User GUID must be 36 characters long.")]
    public required Guid UserGuid { get; set; }

    /// <summary>
    /// The first name of the user.
    /// </summary>
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(255)]
    public required string FirstName { get; set; }

    /// <summary>
    /// The last name of the user.
    /// </summary>
    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(255)]
    public required string LastName { get; set; }

    /// <summary>
    /// The email address of the user.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public required string Email { get; set; }
}
