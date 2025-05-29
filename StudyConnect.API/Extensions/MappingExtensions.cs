using StudyConnect.API.Dtos.Responses.Group;
using StudyConnect.API.Dtos.Responses.User;
using StudyConnect.Core.Models;

namespace StudyConnect.API.Extensions;

/// <summary>
/// Provides extension methods for mapping domain models to Data Transfer Objects (DTOs).
/// </summary>
public static class MappingExtensions
{
    /// <summary>
    /// Converts a <see cref="User"/> domain model to a <see cref="UserReadDto"/>.
    /// </summary>
    /// <param name="user">The user model to convert.</param>
    /// <returns>A <see cref="UserReadDto"/> containing the mapped user data.</returns>
    public static UserReadDto ToUserReadDto(this User user) =>
        new()
        {
            Oid = user.UserGuid,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
        };

    /// <summary>
    /// Converts a <see cref="GroupMember"/> domain model to a <see cref="GroupMemberReadDto"/>.
    /// </summary>
    /// <param name="member">The group member model to convert.</param>
    /// <returns>A <see cref="UserReadDto"/> containing the mapped group member data.</returns>
    public static GroupMemberReadDto ToGroupMemberReadDto(this GroupMember member) =>
        new()
        {
            GroupId = member.GroupId,
            JoinedAt = member.JoinedAt,
            Member = member.Member.ToUserReadDto(),
        };
}
