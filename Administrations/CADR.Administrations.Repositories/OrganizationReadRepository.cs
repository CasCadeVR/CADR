using CADR.Common.Repositories;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CADR.Administrations.Repositories;

/// <inheritdoc cref="IOrganizationReadRepository"/>
internal sealed class OrganizationReadRepository : IOrganizationReadRepository, IAdministrationRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationReadRepository"/>
    /// </summary>
    public OrganizationReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<bool> IOrganizationReadRepository.IsUserAdminForOtherUserOrganizationAsync(
        Guid userId,
        Guid otherUserId,
        CancellationToken cancellationToken)
        => reader.Read<Organization>()
            .Where(x => x.Users.Any(y => y.UserId == userId &&
                                         y.Role == Role.Admin &&
                                         y.DeletedAt == null) &&
                        x.Users.Any(y => y.UserId == otherUserId &&
                                         y.DeletedAt == null))
            .NotDeletedAt()
            .AnyAsync(cancellationToken);

    Task<Organization?> IOrganizationReadRepository.GetActiveByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        => reader.Read<Organization>()
            .ById(id)
            .Where(x => x.Users.Any(y => y.UserId == userId &&
                                         y.DeletedAt == null))
            .NotDeletedAt()
            .FirstOrDefaultAsync(cancellationToken);

    Task<Organization?> IOrganizationReadRepository.GetActiveByNameAsync(string name, CancellationToken cancellationToken)
        => reader.Read<Organization>()
            .Where(x => x.NameLowerCase == name.ToLower())
            .NotDeletedAt()
            .FirstOrDefaultAsync(cancellationToken);

    Task<IReadOnlyCollection<Organization>> IOrganizationReadRepository.GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        => reader.Read<Organization>()
            .Where(x => x.Users.Any(y => y.UserId == userId &&
                                         y.DeletedAt == null))
            .NotDeletedAt()
            .OrderBy(x => x.NameLowerCase)
            .ToReadOnlyCollectionAsync(cancellationToken);

    Task<bool> IOrganizationReadRepository.IsActiveNameExistsAsync(string organizationName, CancellationToken cancellationToken)
        => reader.Read<Organization>()
            .Where(x => x.NameLowerCase == organizationName.ToLower())
            .NotDeletedAt()
            .AnyAsync(cancellationToken);

    Task<Dictionary<Guid, Organization>> IOrganizationReadRepository.GetActiveByCollectionIdAsync(IReadOnlyCollection<Guid> organizationIds, CancellationToken cancellationToken)
        => reader.Read<Organization>()
            .NotDeletedAt()
            .Where(x => organizationIds.Contains(x.Id))
            .OrderBy(x => x.NameLowerCase)
            .ToDictionaryAsync(x => x.Id, x => x, cancellationToken);
}
