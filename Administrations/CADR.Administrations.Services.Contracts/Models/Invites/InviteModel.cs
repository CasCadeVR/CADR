using CADR.Administrations.Services.Contracts.Models.Enums;

namespace CADR.Administrations.Services.Contracts.Models.Invites;

/// <summary>
/// Модель приглашения к вступлению в организацию
/// </summary>
public class InviteModel
{
    /// <summary>
    /// Идентификатор пользователя, создающий приглашение
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Почтовый адрес приглашаемого пользователя
    /// </summary>
    public string UserMail { get; set; } = string.Empty;

    /// <inheritdoc cref="UserRole"/>
    public UserRole Role { get; set; }
}
