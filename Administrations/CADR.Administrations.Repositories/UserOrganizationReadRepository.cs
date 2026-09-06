using System.Linq.Expressions;
using CADR.Common.Repositories;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CADR.Administrations.Repositories;

/// <inheritdoc cref="IUserOrganizationReadRepository"/>
internal sealed class UserOrganizationReadRepository : IUserOrganizationReadRepository, IAdministrationRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserOrganizationReadRepository"/>
    /// </summary>
    public UserOrganizationReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<UserOrganization?> IUserOrganizationReadRepository.GetByUserAndOrganizationIdAsync(Guid userId,
        Guid organizationId,
        CancellationToken cancellationToken)
        => reader.Read<UserOrganization>()
            .Where(x => x.UserId == userId &&
                        x.OrganizationId == organizationId)
            .NotDeletedAt()
            .FirstOrDefaultAsync(cancellationToken);

    Task<Dictionary<Guid, Role>> IUserOrganizationReadRepository.GetRoleByUserAndOrganizationIdsAsync(Guid userId,
        IReadOnlyCollection<Guid> organizationIds,
        CancellationToken cancellationToken)
        => reader.Read<UserOrganization>()
            .Where(x => x.UserId == userId)
            .Where(GetByOrganizationIdsExpression(organizationIds))
            .NotDeletedAt()
            .ToDictionaryAsync(x => x.OrganizationId!.Value, x => x.Role, cancellationToken);

    Task<IReadOnlyCollection<UserOrganization>> IUserOrganizationReadRepository.GetByOrganizationIdAsync(Guid organizationId,
        CancellationToken cancellationToken)
        => reader.Read<UserOrganization>()
            .Where(x => x.OrganizationId == organizationId)
            .NotDeletedAt()
            .ToReadOnlyCollectionAsync(cancellationToken);

    private static Expression<Func<UserOrganization, bool>> GetByOrganizationIdsExpression(IReadOnlyCollection<Guid> ids)
        => ids.Count switch
        {
            0 => x => false,
            1 => x => x.OrganizationId == ids.First(),
            _ => x => x.OrganizationId.HasValue && ids.Contains(x.OrganizationId.Value)
        };
}
