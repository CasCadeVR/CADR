namespace CADR.Adrs.Api.Models.Adrs;

/// <summary>
/// API Модель раздела ADR
/// </summary>
public class AdrSectionApiModel
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
    /// Содержимое раздела
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Подсказка к разделу
    /// </summary>
    /// <remarks>
    /// null - если создание прошло не по шаблону
    /// </remarks>
    public string? Hint { get; set; } = string.Empty;

    /// <summary>
    /// Подсказка к разделу в виде placeholder
    /// </summary>
    /// <remarks>
    /// null - если создание прошло не по шаблону
    /// </remarks>
    public string? Placeholder { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор ADR
    /// </summary>
    public Guid AdrId { get; set; }
}
