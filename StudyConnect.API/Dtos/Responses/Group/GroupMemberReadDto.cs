using System;
using StudyConnect.API.Dtos.Responses.User;

namespace StudyConnect.API.Dtos.Responses.Group;

/// <summary>
/// Data returned to the client for a single group member.
/// </summary>
public class GroupMemberReadDto
{
    /// <summary>
    /// The identifier of the group the member belongs to.
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    /// Timestamp of when the user joined the group.
    /// </summary>
    public DateTime JoinedAt { get; set; }

    /// <summary>
    /// The User information of the member
    /// </summary>
    public UserReadDto? Member { get; set; }
}

