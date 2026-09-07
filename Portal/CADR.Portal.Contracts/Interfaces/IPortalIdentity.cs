using System.Security.Claims;

namespace CADR.Portal.Contracts.Interfaces;

/// <summary>
/// Поставщик личности
/// </summary>
public interface IPortalIdentity
{
    /// <summary>
    /// Пользователь авторизован
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Имя входа пользователя
    /// </summary>
    string Login { get; }

    /// <summary>
    /// Адрес электронной почты
    /// </summary>
    string Email { get; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// Проверяет наличие заданной роли у пользователя
    /// </summary>
    bool IsInRole(string roleName);

    /// <summary>
    /// Список клеймов
    /// </summary>
    IEnumerable<Claim> Claims { get; }

    /// <summary>
    /// Делегат события смены авторизации
    /// </summary>
    Action? OnChangedAction { get; set; }

    /// <summary>
    /// Обновление состояния авторизации 
    /// </summary>
    Task TryAuthenticate(CancellationToken cancellationToken);

    /// <summary>
    /// Инвалидировать кеш текущего пользователя
    /// </summary>
    void InvalidateCache();
}
