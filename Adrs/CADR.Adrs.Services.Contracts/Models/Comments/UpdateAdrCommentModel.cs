namespace CADR.Adrs.Services.Contracts.Models.Comments;

/// <summary>
/// Модель обновления комментария к ADR
/// </summary>
public class UpdateAdrCommentModel : AdrCommentModel
{
    /// <summary>
    /// Идентификатор пользователя, обновляющий комментарий ADR
    /// </summary>
    public Guid UserId { get; set; }
}
