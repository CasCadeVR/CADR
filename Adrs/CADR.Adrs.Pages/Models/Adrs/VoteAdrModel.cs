using CADR.Adrs.Pages.Models.Adrs.Enums;

namespace CADR.Adrs.Pages.Models.Adrs.Adrs;

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
