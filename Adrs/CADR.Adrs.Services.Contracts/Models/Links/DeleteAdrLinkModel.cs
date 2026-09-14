namespace CADR.Adrs.Services.Contracts.Models.Links;

/// <summary>
/// Модель удаления связи ADR
/// </summary>
public class DeleteAdrLinkModel
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
