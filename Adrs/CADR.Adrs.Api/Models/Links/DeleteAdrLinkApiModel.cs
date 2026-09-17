namespace CADR.Adrs.Api.Models.Links;

/// <summary>
/// API Модель удаления связи ADR
/// </summary>
public class DeleteAdrLinkApiModel
{
    /// <summary>
    /// Идентификатор пользователя, удаляющего связь с ADR
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор связи ADR
    /// </summary>
    public Guid AdrLinkId { get; set; }
}
