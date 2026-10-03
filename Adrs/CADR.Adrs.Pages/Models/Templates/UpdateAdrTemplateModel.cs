namespace CADR.Adrs.Pages.Models.Adrs.Templates;

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
