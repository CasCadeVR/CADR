namespace CADR.Adrs.Api.Models.Folders;

/// <summary>
/// API Модель удаления папки ADR
/// </summary>
public class DeleteAdrFolderApiModel
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
