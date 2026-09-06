namespace CADR.Administrations.Api.Models.Users;

/// <summary>
/// Модель ответа авторизации
/// </summary>
public class LoginApiResponse
{
    /// <summary>
    /// Jwt
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Токен обновления
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}
