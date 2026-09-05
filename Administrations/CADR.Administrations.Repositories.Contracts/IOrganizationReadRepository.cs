using CADR.Administrations.Entities;

namespace CADR.Administrations.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="Organization"/>
/// </summary>
public interface IOrganizationReadRepository
{
    /// <summary>
    /// Получает активную <see cref="Organization"/> по идентификатору
    /// </summary>
    Task<Organization?> GetActiveByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает активную <see cref="Organization"/> по имени
    /// </summary>
    Task<Organization?> GetActiveByNameAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    /// Получает словарь из всех активных <see cref="Organization"/> по коллекции идентификаций
    /// </summary>
    Task<Dictionary<Guid, Organization>> GetActiveByCollectionIdAsync(IReadOnlyCollection<Guid> guids, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех организаций пользователя
    /// </summary>
    Task<IReadOnlyCollection<Organization>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Проверяет, существует ли организация с указанным именем
    /// </summary>
    Task<bool> IsActiveNameExistsAsync(string organizationName, CancellationToken cancellationToken);

    /// <summary>
    /// Проверяет, является ли пользователь администратором организации в которой состоит другой пользователь
    /// </summary>
    Task<bool> IsUserAdminForOtherUserOrganizationAsync(Guid userId, Guid otherUserId, CancellationToken cancellationToken);
}
