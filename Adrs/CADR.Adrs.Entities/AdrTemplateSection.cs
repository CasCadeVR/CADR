using CADR.Context.Entities.Contracts.Models;

namespace CADR.Adrs.Entities;

/// <summary>
/// Раздел шаблона ADR
/// </summary>
public class AdrTemplateSection : BaseAuditEntity
{
    /// <summary>
    /// Порядковый номер в ADR
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// Имя раздела
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Подсказка для архитектора - что написать
    /// </summary>
    public string Hint { get; set; } = string.Empty;

    /// <summary>
    /// Содержимое раздела
    /// </summary>
    public string Placeholder { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор шаблона ADR
    /// </summary>
    public Guid TemplateId { get; set; }

    /// <summary>
    /// Навигационное свойство шаблона ADR
    /// </summary>
    public AdrTemplate? Template { get; set; }
}
