namespace CADR.Adrs.Api.Models.Folders;

/// <summary>
/// API Модель создания папки ADR
/// </summary>
public class CreateAdrFolderApiModel
{
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

    /// <summary>
    /// Идентификатор пользователя, создающий папку
    /// </summary>
    public Guid UserId { get; set; }
}
