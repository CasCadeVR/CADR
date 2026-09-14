namespace CADR.Adrs.Services.Contracts.Models.Adrs;

/// <summary>
/// Модель раздела ADR
/// </summary>
public class AdrSectionModel
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
    /// Идентификатор ADR
    /// </summary>
    public Guid AdrId { get; set; }
}
