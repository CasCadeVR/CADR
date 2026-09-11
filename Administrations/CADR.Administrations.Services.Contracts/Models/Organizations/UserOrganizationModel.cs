using CADR.Administrations.Services.Contracts.Models.Enums;

namespace CADR.Administrations.Services.Contracts.Models.Organizations;

/// <summary>
/// Модель пользователя организации
/// </summary>
public class UserOrganizationModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя входа
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Электронный адрес
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Признак заблокированной учётной записи
    /// </summary>
    public bool Blocked { get; set; }

    /// <summary>
    /// Время действия блокировки
    /// </summary>
    public DateTimeOffset? BlockedAt { get; set; }

    /// <summary>
    /// Заметка о пользователе
    /// </summary>
    public string Note { get; set; } = string.Empty;

    /// <inheritdoc cref="UserRole"/>
    public UserRole Role { get; set; }
}
