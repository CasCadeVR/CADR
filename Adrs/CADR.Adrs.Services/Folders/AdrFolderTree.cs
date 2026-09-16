using CADR.Adrs.Entities;

namespace CADR.Adrs.Services.Folders;

/// <summary>
/// Вспомогательные методы для работы с деревом папок ADR
/// </summary>
internal static class AdrFolderTree
{
    /// <summary>
    /// Строит путь к папке от корня организации (цепочку родительских папок)
    /// </summary>
    /// <param name="folder">Папка, для которой строится путь</param>
    /// <param name="folderById">Все активные папки организации по идентификатору</param>
    /// <returns>Папки пути в порядке от корня к указанной папке</returns>
    internal static IReadOnlyCollection<AdrFolder> GetPath(AdrFolder folder, IReadOnlyDictionary<Guid, AdrFolder> folderById)
    {
        var path = new List<AdrFolder>();
        AdrFolder? current = folder;
        while (current != null)
        {
            path.Add(current);
            current = current.ParentAdrFolderId.HasValue && folderById.TryGetValue(current.ParentAdrFolderId.Value, out var parent)
                ? parent
                : null;
        }

        path.Reverse();
        return path;
    }
}
