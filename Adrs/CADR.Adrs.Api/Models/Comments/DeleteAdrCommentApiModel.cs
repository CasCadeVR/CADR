namespace CADR.Adrs.Api.Models.Comments;

/// <summary>
/// API Модель удаления комментария ADR
/// </summary>
public class DeleteAdrCommentApiModel
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
