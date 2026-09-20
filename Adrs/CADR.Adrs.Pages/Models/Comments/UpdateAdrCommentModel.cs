namespace CADR.Adrs.Pages.Models.Adrs.Comments;

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
