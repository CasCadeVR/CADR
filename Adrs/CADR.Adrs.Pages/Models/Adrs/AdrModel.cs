using CADR.Adrs.Pages.Models.Adrs.Adrs;
using CADR.Adrs.Pages.Models.Adrs.Enums;
using CADR.Adrs.Pages.Models.Adrs.Folders;

namespace CADR.Adrs.Pages.Models.Adrs;

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
    /// Имя автора ADR
    /// </summary>
    public string AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// Логин автора ADR
    /// </summary>
    public string AuthorLogin { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор родителя (папки)
    /// </summary>
    public Guid? ParentAdrFolderId { get; set; }

    /// <summary>
    /// Путь к папке ADR от корня организации (для хлебных крошек)
    /// </summary>
    /// <remarks>
    /// Заполняется только в ответах на чтение одного ADR; пустая коллекция - ADR находится в корне
    /// </remarks>
    public IReadOnlyCollection<AdrFolderModel>? FolderPath { get; set; }

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

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
