namespace CADR.Common.Mvc.Models;

/// <summary>
/// Cookie authentication settings model
/// </summary>
public class CookieSettingsModel
{
    /// <summary>
    /// Имя cookie для access token
    /// </summary>
    public string? AccessTokenName { get; set; }

    /// <summary>
    /// Имя cookie для refresh token
    /// </summary>
    public string? RefreshTokenName { get; set; }

    /// <summary>
    /// Путь cookie
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Флаг HttpOnly
    /// </summary>
    public bool HttpOnly { get; set; }

    /// <summary>
    /// Флаг Secure (только по HTTPS)
    /// </summary>
    public bool Secure { get; set; }

    /// <summary>
    /// Политика SameSite
    /// </summary>
    public string? SameSite { get; set; }
}
