using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrOrganizationSettingsReadRepository"/>
internal sealed class AdrOrganizationSettingsReadRepository : IAdrOrganizationSettingsReadRepository, IAdrsRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrOrganizationSettingsReadRepository"/>
    /// </summary>
    public AdrOrganizationSettingsReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<AdrOrganizationSettings?> IAdrOrganizationSettingsReadRepository.GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken)
        => reader.Read<AdrOrganizationSettings>()
            .Where(x => x.OrganizationId == organizationId)
            .NotDeletedAt()
            .FirstOrDefaultAsync(cancellationToken);
}
