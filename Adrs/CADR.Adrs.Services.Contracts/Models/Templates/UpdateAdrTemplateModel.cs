namespace CADR.Adrs.Services.Contracts.Models.Templates;

/// <summary>
/// Модель обновления шаблона ADR
/// </summary>
public class UpdateAdrTemplateModel : AdrTemplateModel
{
    /// <summary>
    /// Идентификатор пользователя, обновляющий шаблон ADR
    /// </summary>
    public Guid UserId { get; set; }
}
