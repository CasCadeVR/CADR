using CADR.Adrs.Pages.Models.Adrs.Enums;

namespace CADR.Adrs.Pages.Models.Adrs.Links;

/// <summary>
/// Модель создания связи с ADR
/// </summary>
public class CreateAdrLinkModel
{
    /// <summary>
    /// Тип связи
    /// </summary>
    public AdrLinkType Type { get; set; }

    /// <summary>
    /// Идентификатор источника ADR
    /// </summary>
    public Guid SourceAdrId { get; set; }

    /// <summary>
    /// Идентификатор указываемого ADR
    /// </summary>
    public Guid TargetAdrId { get; set; }

    /// <summary>
    /// Идентификатор пользователя, создающий связь
    /// </summary>
    public Guid UserId { get; set; }
}
