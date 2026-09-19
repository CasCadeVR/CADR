using CADR.Adrs.Api.Models.Enums;

namespace CADR.Adrs.Api.Models.Adrs;

/// <summary>
/// API Модель голоса ADR
/// </summary>
public class VoteAdrApiModel : WithdrawVoteAdrApiModel
{
    /// <summary>
    /// Голос
    /// </summary>
    public AdrVoteTypeApi Vote { get; init; }
}
