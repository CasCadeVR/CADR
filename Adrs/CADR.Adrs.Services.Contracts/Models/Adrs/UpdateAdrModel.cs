namespace CADR.Adrs.Services.Contracts.Models.Adrs;

/// <summary>
/// Модель обновления ADR
/// </summary>
public class UpdateAdrModel : AdrModel
{
    /// <summary>
    /// Идентификатор пользователя, обновляющий ADR
    /// </summary>
    public Guid UserId { get; set; }
}
