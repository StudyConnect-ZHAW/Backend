using System;

namespace StudyConnect.API.Dtos.Responses.User;

/// <summary>
/// Data transfer object for reading user information.
/// </summary>
public class UserReadDto
{
    /// <summary>
    /// The first name of the user.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// The last name of the user.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// The email address of the user.
    /// </summary>
    public string? Email { get; set; }
}
