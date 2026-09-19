namespace CADR.Adrs.Api.Models.Templates;

/// <summary>
/// API Модель удаления шаблона ADR
/// </summary>
public class DeleteAdrTemplateApiModel
{
    /// <summary>
    /// Идентификатор пользователя, удаляющего шаблон ADR
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор шаблона ADR
    /// </summary>
    public Guid AdrTemplateId { get; set; }
}
