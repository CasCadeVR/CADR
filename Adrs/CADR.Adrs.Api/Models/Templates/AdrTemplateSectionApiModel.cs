namespace CADR.Adrs.Api.Models.Templates;

/// <summary>
/// API Модель раздела шаблона ADR
/// </summary>
public class AdrTemplateSectionApiModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

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
}
