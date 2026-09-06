namespace CADR.Administrations.Services.Contracts.Models.Token;

/// <summary>
/// Модель обновления токена обновления
/// </summary>
public class UpdateRefreshTokenModel
{
    /// <summary>
    /// Идентификатор токена обновления
    /// </summary>
    public Guid TokenId { get; set; }

    /// <summary>
    /// Полезные данные для токена доступа
    /// </summary>
    public string ClaimPayload { get; set; } = string.Empty;
}
