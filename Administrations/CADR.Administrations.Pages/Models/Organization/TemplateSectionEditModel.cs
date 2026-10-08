namespace CADR.Administrations.Pages.Models.Organization;

/// <summary>
/// Модель редактирования секции шаблона ADR
/// </summary>
public class TemplateSectionEditModel
{
    /// <summary>
    /// Заголовок секции
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Подсказка секции
    /// </summary>
    public string Hint { get; set; } = string.Empty;

    /// <summary>
    /// Placeholder секции
    /// </summary>
    public string Placeholder { get; set; } = string.Empty;
}
