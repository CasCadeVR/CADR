namespace CADR.Administrations.Services.Contracts.Models.User;

/// <summary>
/// Модель авторизованного пользователя
/// </summary>
public class UserLoggedModel
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
    /// Адрес электронной почты
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Отметка безопасности
    /// </summary>
    public string SecurityStamp { get; set; } = string.Empty;
}
