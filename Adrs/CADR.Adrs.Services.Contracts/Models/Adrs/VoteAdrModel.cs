using CADR.Adrs.Services.Contracts.Models.Enums;

namespace CADR.Adrs.Services.Contracts.Models.Adrs;

/// <summary>
/// Модель голоса ADR
/// </summary>
public class VoteAdrModel : WithdrawVoteAdrModel
{
    /// <summary>
    /// Голос
    /// </summary>
    public AdrVoteType Vote { get; init; }
}
