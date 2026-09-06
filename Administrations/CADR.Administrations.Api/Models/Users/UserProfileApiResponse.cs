namespace CADR.Administrations.Api.Models.Users;

/// <summary>
/// Модель ответа, представляющая информацию о текущем авторизованном пользователе
/// </summary>
public class UserProfileApiResponse
{
    /// <summary>
    /// Клеймы
    /// </summary>
    public IDictionary<string, string> Claims { get; set; } = default!;
}
