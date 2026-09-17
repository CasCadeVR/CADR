namespace CADR.Adrs.Services.Contracts.Models.Adrs;

/// <summary>
/// Модель снятия голоса с ADR
/// </summary>
public class WithdrawVoteAdrModel
{
    /// <summary>
    /// Идентификатор голосующего пользователя
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Идентификатор ADR
    /// </summary>
    public Guid AdrId { get; init; }
}
