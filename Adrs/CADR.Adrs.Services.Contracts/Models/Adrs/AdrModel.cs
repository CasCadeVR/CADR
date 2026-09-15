using CADR.Adrs.Services.Contracts.Models.Enums;

namespace CADR.Adrs.Services.Contracts.Models.Adrs;

/// <summary>
/// Модель ADR
/// </summary>
public class AdrModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Порядковый номер в организации
    /// </summary>
    public int Number { get; set; }

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

    /// <summary>
    /// Голос запрашивающего пользователя
    /// </summary>
    public AdrVoteType? UserVote { get; set; }

    /// <summary>
    /// Разделы ADR
    /// </summary>
    public IReadOnlyCollection<AdrSectionModel> Sections { get; set; } = [];
}
