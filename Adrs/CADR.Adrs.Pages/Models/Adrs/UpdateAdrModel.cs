using CADR.Adrs.Pages.Models.Adrs;

namespace CADR.Adrs.Pages.Models.Adrs.Adrs;

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
