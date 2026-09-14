namespace CADR.Adrs.Services.Contracts.Models.Templates;

/// <summary>
/// Модель удаления шаблона ADR
/// </summary>
public class DeleteAdrTemplateModel
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
