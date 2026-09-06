using System.Linq.Expressions;
using CADR.Common.Repositories;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CADR.Administrations.Repositories;

/// <inheritdoc cref="IUserReadRepository"/>
internal sealed class UserReadRepository : IUserReadRepository, IAdministrationRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserReadRepository"/>
    /// </summary>
    public UserReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<User?> IUserReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<User>()
            .ById(id)
            .SingleOrDefaultAsync(cancellationToken);

    Task<User?> IUserReadRepository.GetActiveByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<User>()
            .NotDeletedAt()
            .ById(id)
            .Where(x => !x.Blocked)
            .SingleOrDefaultAsync(cancellationToken);

    Task<User?> IUserReadRepository.GetActiveByLoginAsync(string login, CancellationToken cancellationToken)
        => reader.Read<User>()
            .NotDeletedAt()
            .Where(x => x.LoginLowerCase == login.ToLower())
            .SingleOrDefaultAsync(cancellationToken);

    Task<User?> IUserReadRepository.GetActiveByEmailAsync(string email, CancellationToken cancellationToken)
        => reader.Read<User>()
            .NotDeletedAt()
            .Where(x => x.EmailLowerCase == email.ToLower())
            .SingleOrDefaultAsync(cancellationToken);


    Task<bool> IUserReadRepository.IsActiveLoginExistsAsync(string login, CancellationToken cancellationToken)
        => reader.Read<User>()
            .NotDeletedAt()
            .AnyAsync(x => x.LoginLowerCase == login.ToLower(), cancellationToken);

    Task<bool> IUserReadRepository.IsActiveEmailExistsAsync(string email, CancellationToken cancellationToken)
        => reader.Read<User>()
            .NotDeletedAt()
            .AnyAsync(x => x.EmailLowerCase == email.ToLower() &&
                           x.EmailConfirmed,
                cancellationToken);

    Task<IReadOnlyCollection<User>> IUserReadRepository.GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken)
        => reader.Read<User>()
            .Where(x => x.Organizations.Any(y => y.OrganizationId == organizationId))
            .NotDeletedAt()
            .ToReadOnlyCollectionAsync(cancellationToken);

    Task<bool> IUserReadRepository.IsAdminInOrganizationAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken)
        => reader.Read<Organization>()
            .NotDeletedAt()
            .AnyAsync(HasMinRoleInOrganization(userId, organizationId, Role.Admin), cancellationToken);

    private static Expression<Func<Organization, bool>> HasMinRoleInOrganization(Guid userId, Guid organizationId, Role minRole)
        => x => x.Id == organizationId
            && x.Users.Any(y => y.UserId == userId
            && y.Role >= minRole
            && y.DeletedAt == null);
}
