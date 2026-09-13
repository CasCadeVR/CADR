using CADR.Adrs.Entities;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="AdrSection"/>
/// </summary>
public interface IAdrSectionReadRepository
{
    /// <summary>
    /// Получает список всех секций одной ADR
    /// </summary>
    Task<IReadOnlyCollection<AdrSection>> GetByAdrIdAsync(Guid adrId, CancellationToken cancellationToken);
}
