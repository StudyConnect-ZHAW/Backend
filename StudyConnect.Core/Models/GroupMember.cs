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
    /// The User information of the Member.
    /// </summary>
    public required User Member { get; set; }
}
