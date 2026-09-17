using CADR.Adrs.Services.Contracts.Models.Folders;

namespace CADR.Adrs.Services.Contracts.Interfaces;

/// <summary>
/// Управление папками ADR
/// </summary>
public interface IAdrFolderManager
{
    /// <summary>
    /// Создаёт новую папку ADR
    /// </summary>
    Task<AdrFolderModel> CreateAsync(CreateAdrFolderModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех папок ADR организации
    /// </summary>
    Task<IEnumerable<AdrFolderModel>> GetByOrganizationIdAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает путь к папке от корня организации (цепочку родительских папок)
    /// </summary>
    Task<IEnumerable<AdrFolderModel>> GetPathAsync(Guid organizationId, Guid? folderId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет существующую папку ADR (переименование, перемещение)
    /// </summary>
    Task<AdrFolderModel> UpdateAsync(UpdateAdrFolderModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает число ADR, которое будет удалено вместе с папкой (включая вложенные папки)
    /// </summary>
    Task<int> GetAdrCountAsync(Guid organizationId, Guid folderId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет существующую папку ADR вместе с вложенными папками и ADR
    /// </summary>
    Task DeleteAsync(DeleteAdrFolderModel model, CancellationToken cancellationToken);
}
