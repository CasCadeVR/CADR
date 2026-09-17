namespace CADR.Adrs.Services.Contracts.Models.Folders;

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
