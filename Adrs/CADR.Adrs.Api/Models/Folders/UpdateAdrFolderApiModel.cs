namespace CADR.Adrs.Api.Models.Folders;

/// <summary>
/// API Модель обновления папки ADR
/// </summary>
public class UpdateAdrFolderApiModel : AdrFolderApiModel
{
    /// <summary>
    /// Идентификатор пользователя, обновляющий папку ADR
    /// </summary>
    public Guid UserId { get; set; }
}
