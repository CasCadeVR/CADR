namespace CADR.Adrs.Pages.Models.Adrs.Folders;

/// <summary>
/// Модель обновления папки ADR
/// </summary>
public class UpdateAdrFolderModel : AdrFolderModel
{
    /// <summary>
    /// Идентификатор пользователя, обновляющий папку ADR
    /// </summary>
    public Guid UserId { get; set; }
}
