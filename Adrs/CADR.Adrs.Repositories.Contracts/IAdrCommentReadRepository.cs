using CADR.Adrs.Entities;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="AdrComment"/>
/// </summary>
public interface IAdrCommentReadRepository
{
    /// <summary>
    /// Получает активный <see cref="AdrComment"/> по идентификатору
    /// </summary>
    Task<AdrComment?> GetActiveByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех комментариев одной ADR
    /// </summary>
    Task<IReadOnlyCollection<AdrComment>> GetByAdrIdAsync(Guid adrId, CancellationToken cancellationToken);
}
