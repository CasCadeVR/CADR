using CADR.Administrations.Entities;

namespace CADR.Administrations.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="User"/>
/// </summary>
public interface IUserReadRepository
{
    /// <summary>
    /// Получает <see cref="User"/> по идентификатору
    /// </summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получает активного <see cref="User"/> по идентификатору
    /// </summary>
    Task<User?> GetActiveByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получает действующий <see cref="User"/> по логину
    /// </summary>
    Task<User?> GetActiveByLoginAsync(string login, CancellationToken cancellationToken);

    /// <summary>
    /// Получает действующий <see cref="User"/> по адресу электронной почты
    /// </summary>
    Task<User?> GetActiveByEmailAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Проверяет существование логина у активной записи
    /// </summary>
    Task<bool> IsActiveLoginExistsAsync(string login, CancellationToken cancellationToken);

    /// <summary>
    /// Проверяет существование подтверждённого адреса почты у активной записи
    /// </summary>
    Task<bool> IsActiveEmailExistsAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех пользователей организации
    /// </summary>
    Task<IReadOnlyCollection<User>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken);

    /// <summary>
    /// Является ли пользователь админом в организации
    /// </summary>
    Task<bool> IsAdminInOrganizationAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken);
}
