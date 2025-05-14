using System;

namespace StudyConnect.Core.Models;

public class User
{
    public Guid UserGuid { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }

    public UserRole? userRole { get; set; }
}
