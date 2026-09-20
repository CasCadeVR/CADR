namespace CADR.Adrs.Api.Models.Adrs;

/// <summary>
/// API Модель обновления ADR
/// </summary>
public class UpdateAdrApiModel : AdrApiModel
{
    /// <summary>
    /// Идентификатор пользователя, обновляющий ADR
    /// </summary>
    public Guid UserId { get; set; }
}
