using CADR.Administrations.Entities;
using CADR.Adrs.Entities.Enums;
using CADR.Context.Entities.Contracts.Models;

namespace CADR.Adrs.Entities;

/// <summary>
/// ADR - Architecture Decision Record (Запись об архитектурном решении)
/// По своей сути коллекция разделов
/// </summary>
public class Adr : BaseAuditEntity
{
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
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Навигационное свойство организации
    /// </summary>
    public Organization? Organization { get; set; }

    /// <summary>
    /// Идентификатор родителя (папки)
    /// </summary>
    public Guid? ParentAdrFolderId { get; set; }

    /// <summary>
    /// Навигационное свойство родителя (папки)
    /// </summary>
    public AdrFolder? ParentAdrFolder { get; set; }

    /// <summary>
    /// Идентификатор шаблона, по которому построен ADR
    /// </summary>
    public Guid? TemplateId { get; set; }

    /// <summary>
    /// Навигационное свойство шаблона, по которому построен ADR
    /// </summary>
    public AdrTemplate? Template { get; set; }

    /// <summary>
    /// Разделы ADR
    /// </summary>
    public ICollection<AdrSection> Sections { get; set; } = [];

    /// <summary>
    /// Связи с другими ADR
    /// </summary>
    public ICollection<AdrLink> TargetLinks { get; set; } = [];

    /// <summary>
    /// Комментарии ADR
    /// </summary>
    public ICollection<AdrComment> Comments { get; set; } = [];

    /// <summary>
    /// Голосование ADR
    /// </summary>
    public ICollection<AdrVote> Votes { get; set; } = [];
}
