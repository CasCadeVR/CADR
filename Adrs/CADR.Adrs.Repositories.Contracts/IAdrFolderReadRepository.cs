using CADR.Adrs.Entities;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="AdrFolder"/>
/// </summary>
public interface IAdrFolderReadRepository
{
    /// <summary>
    /// Получает активную <see cref="AdrFolder"/> по идентификатору
    /// </summary>
    Task<AdrFolder?> GetActiveByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех папок ADR организации
    /// </summary>
    Task<IReadOnlyCollection<AdrFolder>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken);

    /// <summary>
    /// Существует ли папка ADR с таким именем в одной папке
    /// </summary>
    Task<bool> IsActiveNameExistsAsync(Guid organizationId, Guid? parentFolderId, string name, CancellationToken cancellationToken);
}
