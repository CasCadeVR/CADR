using CADR.Adrs.Pages.Models.Adrs.Enums;

namespace CADR.Adrs.Pages.Models.Adrs.Adrs;

/// <summary>
/// Модель создания ADR
/// </summary>
public class CreateAdrModel
{
    /// <summary>
    /// Имя ADR
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Статус ADR
    /// </summary>
    public AdrStatus Status { get; set; }

    /// <summary>
    /// Текущий авторитет ADR
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Идентификатор автора ADR
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Идентификатор родителя (папки)
    /// </summary>
    public Guid? ParentAdrFolderId { get; set; }

    /// <summary>
    /// Идентификатор шаблона, по которому построен ADR
    /// </summary>
    public Guid? TemplateId { get; set; }
}
