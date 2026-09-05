using CADR.Context.Entities.Contracts.Models;

namespace CADR.Administrations.Entities;

/// <summary>
/// Пользователь системы
/// </summary>
public class User : BaseAuditEntity
{
    /// <summary>
    /// Имя входа
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Значение <see cref="Login"/> в нижнем регистре
    /// </summary>
    public string LoginLowerCase { get; set; } = string.Empty;

    /// <summary>
    /// Хеш пароля
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Соль пароля
    /// </summary>
    public string PasswordSalt { get; set; } = string.Empty;

    /// <summary>
    /// Отметка безопасности
    /// </summary>
    public string SecurityStamp { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Электронный адрес
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Значение <see cref="Email"/> в нижнем регистре
    /// </summary>
    public string EmailLowerCase { get; set; } = string.Empty;

    /// <summary>
    /// Подтверждение электронного адреса
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// Признак заблокированной учётной записи
    /// </summary>
    public bool Blocked { get; set; }

    /// <summary>
    /// Время действия блокировки
    /// </summary>
    public DateTimeOffset? BlockedAt { get; set; }

    /// <summary>
    /// Организации пользователя
    /// </summary>
    public ICollection<UserOrganization> Organizations { get; set; }

    /// <summary>
    /// Приглашения пользователя
    /// </summary>
    public ICollection<UserInvite> Invites { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="User"/>
    /// </summary>
    public User()
    {
        Organizations = new List<UserOrganization>();
        Invites = new List<UserInvite>();
    }
}
