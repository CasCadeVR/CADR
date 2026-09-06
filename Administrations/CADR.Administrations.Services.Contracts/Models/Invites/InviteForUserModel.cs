using CADR.Administrations.Services.Contracts.Models.Enums;

namespace CADR.Administrations.Services.Contracts.Models.Invites;

/// <summary>
/// Модель приглашения к вступлению в организацию, адресованное пользователю
/// </summary>
public class InviteForUserModel
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

    /// <inheritdoc cref="UserRole"/>
    public UserRole Role { get; set; }
}
