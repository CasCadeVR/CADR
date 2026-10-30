using CADR.Administrations.Entities.Views;
using CADR.Administrations.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;

namespace CADR.Administrations.Repositories;

/// <inheritdoc cref="IOrganizationMemberViewReadRepository"/>
internal sealed class OrganizationMemberViewReadRepository
    : IOrganizationMemberViewReadRepository, IAdministrationRepositoryAnchor
{
    private readonly IReader reader;

    public OrganizationMemberViewReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<IReadOnlyCollection<OrganizationMemberView>> IOrganizationMemberViewReadRepository.GetByOrganizationIdAsync(Guid organizationId, CancellationToken ct)
        => reader.Read<OrganizationMemberView>()
            .Where(x => x.OrganizationId == organizationId)
            .NotDeletedAt()
            .OrderBy(x => x.UserName.ToLower())
            .ToReadOnlyCollectionAsync(ct);
}
