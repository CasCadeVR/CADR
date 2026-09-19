namespace CADR.Adrs.Api.Models.Templates;

/// <summary>
/// API Модель обновления шаблона ADR
/// </summary>
public class UpdateAdrTemplateApiModel : AdrTemplateApiModel
{
    /// <summary>
    /// Идентификатор пользователя, обновляющий шаблон ADR
    /// </summary>
    public Guid UserId { get; set; }
}
