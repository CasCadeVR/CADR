using CADR.Administrations.Api.Models.Enums;

namespace CADR.Administrations.Api.Models.Organizations;

/// <summary>
/// Модель пользователя организации
/// </summary>
public class UserOrganizationApiModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

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

    /// <inheritdoc cref="UserRoleApi"/>
    public UserRoleApi Role { get; set; }
}
