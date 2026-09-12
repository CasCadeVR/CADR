using CADR.Administrations.Entities;
using CADR.Adrs.Entities.Enums;
using CADR.Context.Entities.Contracts.Models;

namespace CADR.Adrs.Entities;

/// <summary>
/// Голос к ADR
/// </summary>
public class AdrVote : BaseAuditEntity
{
    /// <summary>
    /// Голос
    /// </summary>
    public AdrVoteType Value { get; set; }

    /// <summary>
    /// Идентификатор ADR
    /// </summary>
    public Guid AdrId { get; set; }

    /// <summary>
    /// Навигационное свойство ADR
    /// </summary>
    public Adr? Adr { get; set; }

    /// <summary>
    /// Идентификатор голосующего
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Навигационное свойство голосующего
    /// </summary>
    public User? User { get; set; }
}
