using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrTemplateReadRepository"/>
internal sealed class AdrTemplateReadRepository : IAdrTemplateReadRepository, IAdrsRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrTemplateReadRepository"/>
    /// </summary>
    public AdrTemplateReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<AdrTemplate?> IAdrTemplateReadRepository.GetActiveByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<AdrTemplate>()
            .ById(id)
            .NotDeletedAt()
            .SingleOrDefaultAsync(cancellationToken);

    Task<IReadOnlyCollection<AdrTemplate>> IAdrTemplateReadRepository.GetAvailableForOrganizationAsync(Guid organizationId, CancellationToken cancellationToken)
        => reader.Read<AdrTemplate>()
            .Where(x => x.OrganizationId == organizationId || x.OrganizationId == null)
            .NotDeletedAt()
            .OrderBy(x => x.Name)
            .ToReadOnlyCollectionAsync(cancellationToken);

    Task<bool> IAdrTemplateReadRepository.IsActiveNameExistsAsync(Guid organizationId, string name, CancellationToken cancellationToken)
        => reader.Read<AdrTemplate>()
            .Where(x => x.OrganizationId == organizationId)
            .Where(x => x.Name.ToLower() == name.ToLower())
            .NotDeletedAt()
            .AnyAsync(cancellationToken);
}
