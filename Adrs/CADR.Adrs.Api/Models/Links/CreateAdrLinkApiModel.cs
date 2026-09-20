using CADR.Adrs.Api.Models.Enums;

namespace CADR.Adrs.Api.Models.Links;

/// <summary>
/// API Модель создания связи с ADR
/// </summary>
public class CreateAdrLinkApiModel
{
    /// <summary>
    /// Тип связи
    /// </summary>
    public AdrLinkTypeApi Type { get; set; }

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
