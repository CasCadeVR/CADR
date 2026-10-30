using CADR.Adrs.Entities.Views;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrSummaryViewReadRepository"/>
internal sealed class AdrSummaryViewReadRepository : IAdrSummaryViewReadRepository, IAdrsRepositoryAnchor
{
    private readonly IReader reader;

    public AdrSummaryViewReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<IReadOnlyCollection<AdrSummaryView>> IAdrSummaryViewReadRepository.GetByIdsAsync(IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken)
        => reader.Read<AdrSummaryView>()
            .Where(x => ids.Contains(x.Id))
            .NotDeletedAt()
            .OrderBy(x => x.Number)
            .ToReadOnlyCollectionAsync(cancellationToken);

    Task<AdrSummaryView?> IAdrSummaryViewReadRepository.GetByAdrIdAsync(Guid adrId, CancellationToken cancellationToken)
        => reader.Read<AdrSummaryView>()
            .Where(x => x.Id == adrId)
            .SingleOrDefaultAsync(cancellationToken);

    Task<IReadOnlyCollection<AdrSummaryView>> IAdrSummaryViewReadRepository.GetByOrganizationIdAsync(
        Guid organizationId, CancellationToken cancellationToken)
        => reader.Read<AdrSummaryView>()
            .Where(x => x.OrganizationId == organizationId)
            .NotDeletedAt()
            .OrderBy(x => x.Number)
            .ToReadOnlyCollectionAsync(cancellationToken);
}
