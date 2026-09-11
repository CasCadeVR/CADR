namespace CADR.Administrations.Api.Models.Users;

/// <summary>
/// Запрос авторизации
/// </summary>
public class LoginApiRequest
{
    /// <summary>
    /// Имя входа
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Запомнить меня
    /// </summary>
    public bool IsRemember { get; set; }
}
