using CADR.Administrations.Entities;

namespace CADR.Administrations.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="UserInvite"/>
/// </summary>
public interface IUserInviteReadRepository
{
    /// <summary>
    /// Получает действующий <see cref="UserInvite"/> по идентификатору
    /// </summary>
    Task<UserInvite?> GetActualByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список действующих <see cref="UserInvite" /> по идентификатору организации
    /// </summary>
    Task<IReadOnlyCollection<UserInvite>> GetActualByOrganizationIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получает действующий <see cref="UserInvite"/> по идентификатору пользователя и организации
    /// </summary>
    Task<UserInvite?> GetActualByUserAndOrganizationIdAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список действующих <see cref="UserInvite"/> по идентификатору организации
    /// включая информацию о пользователе
    /// </summary>
    Task<IReadOnlyCollection<UserInvite>> GetActualWthUserByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список действующих <see cref="UserInvite"/> по идентификатору пользователя
    /// </summary>
    Task<IReadOnlyCollection<UserInvite>> GetActualByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
