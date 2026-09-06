namespace CADR.Administrations.Api.Models.Users;

/// <summary>
/// Запрос обновления токена доступа
/// </summary>
public class RefreshTokenApiRequest
{
    /// <summary>
    /// Токен обновления
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}
