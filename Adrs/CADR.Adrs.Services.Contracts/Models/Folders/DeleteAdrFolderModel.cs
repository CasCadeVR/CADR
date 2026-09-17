namespace CADR.Adrs.Services.Contracts.Models.Folders;

/// <summary>
/// Модель удаления папки ADR
/// </summary>
public class DeleteAdrFolderModel
{
    /// <summary>
    /// Идентификатор пользователя, удаляющего папку ADR
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор папки ADR
    /// </summary>
    public Guid AdrFolderId { get; set; }
}
