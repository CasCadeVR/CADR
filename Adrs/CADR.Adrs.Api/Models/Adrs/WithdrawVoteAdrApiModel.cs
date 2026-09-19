namespace CADR.Adrs.Api.Models.Adrs;

/// <summary>
/// API Модель снятия голоса с ADR
/// </summary>
public class WithdrawVoteAdrApiModel
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
