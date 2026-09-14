using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrSectionReadRepository"/>
internal sealed class AdrSectionReadRepository : IAdrSectionReadRepository, IAdrsRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrSectionReadRepository"/>
    /// </summary>
    public AdrSectionReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<IReadOnlyCollection<AdrSection>> IAdrSectionReadRepository.GetByAdrIdAsync(Guid adrId, CancellationToken cancellationToken)
        => reader.Read<AdrSection>()
            .Where(x => x.AdrId == adrId)
            .NotDeletedAt()
            .OrderBy(x => x.Position)
            .ToReadOnlyCollectionAsync(cancellationToken);
}
