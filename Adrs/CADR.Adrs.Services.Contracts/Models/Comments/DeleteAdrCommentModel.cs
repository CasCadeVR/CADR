namespace CADR.Adrs.Services.Contracts.Models.Comments;

/// <summary>
/// Модель удаления комментария ADR
/// </summary>
public class DeleteAdrCommentModel
{
    /// <summary>
    /// Идентификатор пользователя, удаляющего комментарий к ADR
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор комментария ADR
    /// </summary>
    public Guid AdrCommentId { get; set; }
}
