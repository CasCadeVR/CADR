namespace CADR.Administrations.Api.Models.Users;

/// <summary>
/// API модель создания пользователя
/// </summary>
public class CreateUserApiRequest
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
    /// Пароль ещё раз
    /// </summary>
    public string PasswordAgain { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Электронный адрес
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
