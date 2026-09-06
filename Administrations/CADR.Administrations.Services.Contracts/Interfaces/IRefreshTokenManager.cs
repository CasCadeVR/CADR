using CADR.Administrations.Services.Contracts.Models.Token;

namespace CADR.Administrations.Services.Contracts.Interfaces;

/// <summary>
/// Управление токенами доступа пользователя
/// </summary>
public interface IRefreshTokenManager
{
    /// <summary>
    /// Создаёт токен обновления и возвращает его идентификатор
    /// </summary>
    Task<Guid> CreateRefreshTokenAsync(CreateRefreshTokenModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет текущий токен обновления и возвращает его payload
    /// </summary>
    Task<UpdateRefreshTokenModel> UpdateRefreshTokenAsync(Guid tokenId, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет токен обновления
    /// </summary>
    Task DeleteRefreshTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
