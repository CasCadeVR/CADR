using CADR.Administrations.Services.Contracts.Models.Enums;

namespace CADR.Administrations.Services.Contracts.Models.Invites;

/// <summary>
/// Модель приглашения организации
/// </summary>
public class InviteOrganizationModel
{
    /// <summary>
    /// Идентификатор приглашения
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя приглашенного пользователя
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Почтовый адрес приглашенного пользователя
    /// </summary>
    public string UserMail { get; set; } = string.Empty;

    /// <inheritdoc cref="UserRole"/>
    public UserRole Role { get; set; }

    /// <summary>
    /// Когда добавлен пользователь
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Кем добавлен пользователь
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
}
