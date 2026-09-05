using Cadr.Common.Repositories;
using CADR.Administrations.Entities;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CADR.Administrations.Repositories;

/// <inheritdoc cref="IUserInviteReadRepository"/>
internal class UserInviteReadRepository : IUserInviteReadRepository, IAdministrationRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserInviteReadRepository"/>
    /// </summary>
    public UserInviteReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<UserInvite?> IUserInviteReadRepository.GetActualByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<UserInvite>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

    Task<IReadOnlyCollection<UserInvite>> IUserInviteReadRepository.GetActualByOrganizationIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<UserInvite>()
            .NotDeletedAt()
            .Where(x => x.OrganizationId == id)
            .ToReadOnlyCollectionAsync(cancellationToken);

    Task<UserInvite?> IUserInviteReadRepository.GetActualByUserAndOrganizationIdAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken)
        => reader.Read<UserInvite>()
            .NotDeletedAt()
            .Where(x => x.UserId == userId && x.OrganizationId == organizationId)
            .FirstOrDefaultAsync(cancellationToken);

    Task<IReadOnlyCollection<UserInvite>> IUserInviteReadRepository.GetActualWthUserByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken)
        => reader.Read<UserInvite>()
            .NotDeletedAt()
            .Where(x => x.OrganizationId == organizationId)
            .Include(x => x.User)
            .ToReadOnlyCollectionAsync(cancellationToken);

    Task<IReadOnlyCollection<UserInvite>> IUserInviteReadRepository.GetActualByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        => reader.Read<UserInvite>()
            .NotDeletedAt()
            .Where(x => x.UserId == userId)
            .ToReadOnlyCollectionAsync(cancellationToken);
}
