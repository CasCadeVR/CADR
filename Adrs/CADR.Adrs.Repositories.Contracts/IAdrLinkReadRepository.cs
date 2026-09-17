using CADR.Adrs.Entities;
using CADR.Adrs.Entities.Enums;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="AdrLink"/>
/// </summary>
public interface IAdrLinkReadRepository
{
    /// <summary>
    /// Получает активную <see cref="AdrLink"/> по идентификатору
    /// </summary>
    Task<AdrLink?> GetActiveByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех связей, использующих этот ADR
    /// </summary>
    Task<IReadOnlyCollection<AdrLink>> GetBySourceAdrIdAsync(Guid adrId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех связей, который этот ADR указывает
    /// </summary>
    Task<IReadOnlyCollection<AdrLink>> GetByTargetAdrIdAsync(Guid adrId, CancellationToken cancellationToken);

    /// <summary>
    /// Существует ли связь с указанными ADR
    /// </summary>
    Task<bool> IsActiveLinkExistsAsync(Guid sourceAdrId, Guid targetAdrId, AdrLinkType type, CancellationToken cancellationToken);
}
