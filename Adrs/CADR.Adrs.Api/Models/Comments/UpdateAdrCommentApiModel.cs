namespace CADR.Adrs.Api.Models.Comments;

/// <summary>
/// API Модель обновления комментария к ADR
/// </summary>
public class UpdateAdrCommentApiModel : AdrCommentApiModel
{
    /// <summary>
    /// Идентификатор пользователя, обновляющий комментарий ADR
    /// </summary>
    public Guid UserId { get; set; }
}
