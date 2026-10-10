using CADR.Adrs.Entities.Enums;
using CADR.Context.Entities.Contracts.Interfaces;

namespace CADR.Adrs.Entities.Views;

/// <summary>
/// Вид подтянутой Adr с использованием VIEW
/// </summary>
public class AdrSummaryView : IEntity, IEntityAuditDeletedAt
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
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Идентификатор автора ADR
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Имя автора
    /// </summary>
    public string AuthorName { get; set; } = null!;

    /// <summary>
    /// Логин автора
    /// </summary>
    public string AuthorLogin { get; set; } = null!;

    /// <summary>
    /// Репутация Adr
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Сколько нужно лайков на одобрение
    /// </summary>
    public int LikesRequiredForApproval { get; set; }

    /// <summary>
    /// Идентификатор родителя (папки)
    /// </summary>
    public Guid? FolderId { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Дата удаления (если soft deleted)
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    DateTimeOffset? IEntityAuditDeletedAt.DeletedAt { get => DeletedAt; set => throw new NotImplementedException(); }
}
