using CADR.Administrations.Services.Contracts.Models.Enums;

namespace CADR.Administrations.Services.Contracts.Models.User;

/// <summary>
/// Информация о пользователе
/// </summary>
public class UserInfoModel
{
    /// <summary>
    /// Идентификатор
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
    /// Дата изменения
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Кто изменил
    /// </summary>
    public string UpdatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Дата удаления
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Роль пользователя
    /// </summary>
    public UserRole Role { get; set; }
}
