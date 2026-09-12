using CADR.Administrations.Entities;
using CADR.Context.Entities.Contracts.Models;

namespace CADR.Adrs.Entities;

/// <summary>
/// Комментарий к ADR
/// </summary>
public class AdrComment : BaseAuditEntity
{
    /// <summary>
    /// Содержимое комментария
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор ADR
    /// </summary>
    public Guid AdrId { get; set; }

    /// <summary>
    /// Навигационное свойство ADR
    /// </summary>
    public Adr? Adr { get; set; }

    /// <summary>
    /// Идентификатор комментирующего
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Навигационное свойство комментирующего
    /// </summary>
    public User? User { get; set; }
}
