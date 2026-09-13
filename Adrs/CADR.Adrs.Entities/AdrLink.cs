using CADR.Adrs.Entities.Enums;
using CADR.Context.Entities.Contracts.Models;

namespace CADR.Adrs.Entities;

/// <summary>
/// Связь к ADR
/// </summary>
public class AdrLink : BaseAuditEntity
{
    /// <summary>
    /// Голос
    /// </summary>
    public AdrLinkType Type { get; set; }

    /// <summary>
    /// Идентификатор источника ADR
    /// </summary>
    public Guid SourceAdrId { get; set; }

    /// <summary>
    /// Навигационное свойство источника ADR
    /// </summary>
    public Adr? SourceAdr { get; set; }

    /// <summary>
    /// Идентификатор указываемого ADR
    /// </summary>
    public Guid TargetAdrId { get; set; }

    /// <summary>
    /// Навигационное свойство указываемого ADR
    /// </summary>
    public Adr? TargetAdr { get; set; }
}
