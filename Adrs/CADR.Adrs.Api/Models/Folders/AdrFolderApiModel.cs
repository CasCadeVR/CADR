namespace CADR.Adrs.Api.Models.Folders;

/// <summary>
/// API Модель папки ADR
/// </summary>
public class AdrFolderApiModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя папки
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Идентификатор родителя (папки)
    /// </summary>
    public Guid? ParentAdrFolderId { get; set; }
}
