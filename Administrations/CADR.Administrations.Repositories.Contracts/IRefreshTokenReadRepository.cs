using CADR.Administrations.Entities;

namespace CADR.Administrations.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="RefreshToken"/>
/// </summary>
public interface IRefreshTokenReadRepository
{
    /// <summary>
    /// Получает <see cref="RefreshToken"/> по идентификатору
    /// </summary>
    Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получает действующий <see cref="RefreshToken"/> по идентификатору
    /// </summary>
    Task<RefreshToken?> GetActualByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получает действующий <see cref="RefreshToken"/> по идентификатору пользователя
    /// </summary>
    Task<RefreshToken?> GetActualByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
