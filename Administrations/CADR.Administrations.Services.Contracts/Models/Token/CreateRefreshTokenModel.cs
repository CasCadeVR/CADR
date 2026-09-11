namespace CADR.Administrations.Services.Contracts.Models.Token;

/// <summary>
/// Модель токена обновления
/// </summary>
public class CreateRefreshTokenModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Полезная информация токена доступа
    /// </summary>
    public string AccessPayload { get; set; } = string.Empty;

    /// <summary>
    /// Отметка безопасности
    /// </summary>
    public string SecurityStamp { get; set; } = string.Empty;

    /// <summary>
    /// Количество дней действия токена обновления
    /// </summary>
    public int ExpiredDays { get; set; }
}
