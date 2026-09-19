namespace CADR.Adrs.Api.Models.Templates;

/// <summary>
/// API Модель создания раздела шаблона ADR
/// </summary>
public class CreateAdrTemplateSectionApiModel
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
}
