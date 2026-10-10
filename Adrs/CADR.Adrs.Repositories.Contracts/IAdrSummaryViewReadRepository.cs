using CADR.Adrs.Entities.Views;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="AdrSummaryView"/>
/// </summary>
public interface IAdrSummaryViewReadRepository
{
    /// <summary>
    /// Получает <see cref="AdrSummaryView"/> по идентификатору
    /// </summary>
    Task<AdrSummaryView?> GetByAdrIdAsync(Guid adrId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список <see cref="AdrSummaryView"/> по идентификаторам
    /// </summary>
    Task<IReadOnlyCollection<AdrSummaryView>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех <see cref="AdrSummaryView"/> по идентификатору организации
    /// </summary>
    Task<IReadOnlyCollection<AdrSummaryView>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken);
}
