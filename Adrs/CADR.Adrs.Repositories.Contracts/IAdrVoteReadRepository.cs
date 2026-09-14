using CADR.Adrs.Entities;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="AdrVote"/>
/// </summary>
public interface IAdrVoteReadRepository
{
    /// <summary>
    /// Получает <see cref="AdrVote"/> по идентификатору ADR и голосующего
    /// </summary>
    Task<AdrVote?> GetByAdrAndUserIdAsync(Guid adrId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех голосов одной ADR
    /// </summary>
    Task<IReadOnlyCollection<AdrVote>> GetByAdrIdAsync(Guid adrId, CancellationToken cancellationToken);
}
