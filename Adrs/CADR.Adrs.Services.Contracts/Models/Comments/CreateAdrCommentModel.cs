namespace CADR.Adrs.Services.Contracts.Models.Comments;

/// <summary>
/// Модель создания комментария ADR
/// </summary>
public class CreateAdrCommentModel
{
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
    public Guid UserId { get; set; }
}
