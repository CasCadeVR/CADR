using CADR.Adrs.Services.Contracts.Models.Enums;

namespace CADR.Adrs.Services.Contracts.Models.Links;

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
