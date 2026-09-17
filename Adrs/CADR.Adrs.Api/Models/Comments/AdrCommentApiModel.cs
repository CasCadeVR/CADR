namespace CADR.Adrs.Api.Models.Comments;

/// <summary>
/// API Модель комментария ADR
/// </summary>
public class AdrCommentApiModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Содержимое комментария
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор ADR
    /// </summary>
    public Guid AdrId { get; set; }

    /// <summary>
    /// Идентификатор комментирующего
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Имя комментирующего
    /// </summary>
    public string AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// Логин комментирующего
    /// </summary>
    public string AuthorLogin { get; set; } = string.Empty;

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
