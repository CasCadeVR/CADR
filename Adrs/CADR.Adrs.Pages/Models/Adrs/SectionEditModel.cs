namespace CADR.Adrs.Pages.Models.Adrs;

/// <summary>
/// Модель редактирования секции ADR
/// </summary>
public class SectionEditModel
{
    /// <summary>
    /// Заголовок секции
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Содержимое секции
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Подксказка от шаблона
    /// </summary>
    public string Hint { get; set; } = string.Empty;

    /// <summary>
    /// Placeholder от шаблона
    /// </summary>
    public string Placeholder { get; set; } = string.Empty;
}
