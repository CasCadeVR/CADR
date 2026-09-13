using CADR.Context.Entities.Contracts.Models;

namespace CADR.Adrs.Entities;

/// <summary>
/// Секция ADR - где храняться данные ADR
/// </summary>
public class AdrSection : BaseAuditEntity
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
    /// Содержимое раздела
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор ADR
    /// </summary>
    public Guid AdrId { get; set; }

    /// <summary>
    /// Навигационное свойство ADR
    /// </summary>
    public Adr? Adr { get; set; }
}
