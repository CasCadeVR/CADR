using CADR.Administrations.Api.Models.Enums;

namespace CADR.Administrations.Api.Models.Invites;

/// <summary>
/// Приглашение к организации, адресованное пользователю
/// </summary>
public class InviteForUserApiResponse
{
    /// <summary>
    /// Идентификатор приглашения
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Название организации
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;

    /// <summary>
    /// Описание организации
    /// </summary>
    public string OrganizationDescription { get; set; } = string.Empty;

    /// <inheritdoc cref="UserRoleApi"/>
    public UserRoleApi Role { get; set; }
}
