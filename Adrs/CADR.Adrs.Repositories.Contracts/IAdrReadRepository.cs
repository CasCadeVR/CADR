using CADR.Adrs.Entities;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="Adr"/>
/// </summary>
public interface IAdrReadRepository
{
    /// <summary>
    /// Получает активную <see cref="Adr"/> по идентификатору
    /// </summary>
    Task<Adr?> GetActiveByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех ADR организации
    /// </summary>
    Task<IReadOnlyCollection<Adr>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список ADR из папки
    /// </summary>
    Task<IReadOnlyCollection<Adr>> GetByFolderIdAsync(Guid organizationId, Guid? folderId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список ADR, созданные автором
    /// </summary>
    Task<IReadOnlyCollection<Adr>> GetByAuthorIdAsync(Guid organizationId, Guid authorId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает максимальный номер последней ADR
    /// </summary>
    Task<int> GetMaxNumberAsync(Guid organizationId, CancellationToken cancellationToken);

    /// <summary>
    /// Существует ли ADR с таким номером
    /// </summary>
    Task<bool> IsActiveNumberExistsAsync(Guid organizationId, int number, CancellationToken cancellationToken);

    /// <summary>
    /// Получает число всех ADR, содержащихся в указанных папках
    /// </summary>
    Task<int> GetCountByFolderIdsAsync(IReadOnlyCollection<Guid> folderIds, CancellationToken cancellationToken);
}
