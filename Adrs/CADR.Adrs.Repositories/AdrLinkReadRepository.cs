using CADR.Adrs.Entities;
using CADR.Adrs.Entities.Enums;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrLinkReadRepository"/>
internal sealed class AdrLinkReadRepository : IAdrLinkReadRepository, IAdrsRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrLinkReadRepository"/>
    /// </summary>
    public AdrLinkReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<AdrLink?> IAdrLinkReadRepository.GetActiveByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<AdrLink>()
            .ById(id)
            .NotDeletedAt()
            .SingleOrDefaultAsync(cancellationToken);

    Task<IReadOnlyCollection<AdrLink>> IAdrLinkReadRepository.GetBySourceAdrIdAsync(Guid adrId, CancellationToken cancellationToken)
        => reader.Read<AdrLink>()
            .Where(x => x.SourceAdrId == adrId)
            .NotDeletedAt()
            .ToReadOnlyCollectionAsync(cancellationToken);

    Task<IReadOnlyCollection<AdrLink>> IAdrLinkReadRepository.GetByTargetAdrIdAsync(Guid adrId, CancellationToken cancellationToken)
          => reader.Read<AdrLink>()
            .Where(x => x.TargetAdrId == adrId)
            .NotDeletedAt()
            .ToReadOnlyCollectionAsync(cancellationToken);

    Task<bool> IAdrLinkReadRepository.IsActiveLinkExistsAsync(Guid sourceAdrId, Guid targetAdrId, AdrLinkType type, CancellationToken cancellationToken)
        => reader.Read<AdrLink>()
                .Where(x => x.SourceAdrId == sourceAdrId)
                .Where(x => x.TargetAdrId == targetAdrId)
                .Where(x => x.Type == type)
                .NotDeletedAt()
                .AnyAsync(cancellationToken);
}
