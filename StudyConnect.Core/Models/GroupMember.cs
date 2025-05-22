using System;

namespace StudyConnect.Core.Models;

public class GroupMember
{
    /// <summary>
    /// The MemberId of the GroupMember.
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// The GroupId of the GroupMember.
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    /// The JoinedAt of the GroupMember.
    /// </summary>
    public DateTime JoinedAt { get; set; }

    /// <summary>
    /// The FirstName of the GroupMember.
    /// </summary>
    public String FirstName { get; set; }

    /// <summary>
    /// The LastName of the GroupMember.
    /// </summary>
    public String LastName { get; set; }

    /// <summary>
    /// The Email of the GroupMember.
    /// </summary>
    public String Email { get; set; }
}
