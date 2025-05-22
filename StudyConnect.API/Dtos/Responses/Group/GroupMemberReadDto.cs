using System;

namespace StudyConnect.API.Dtos.Responses.Group;

/// <summary>
/// Data returned to the client for a single group member.
/// </summary>
public class GroupMemberReadDto
{
    /// <summary>
    /// The user’s unique identifier.
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// The identifier of the group the member belongs to.
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    /// Timestamp of when the user joined the group.
    /// </summary>
    public DateTime JoinedAt { get; set; }

    /// <summary>
    /// FirstName of when the user joined the group.
    /// </summary>
    public String FirstName { get; set; }

    /// <summary>
    /// LastName of when the user joined the group.
    /// </summary>
    public String LastName { get; set; }
    /// <summary>
    /// Email of when the user joined the group.
    /// </summary>
    public String Email { get; set; }
    
}