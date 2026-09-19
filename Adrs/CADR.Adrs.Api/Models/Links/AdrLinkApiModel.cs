using CADR.Adrs.Api.Models.Enums;

namespace CADR.Adrs.Api.Models.Links;

/// <summary>
/// API Модель связи ADR
/// </summary>
public class AdrLinkApiModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

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
    /// Номер указываемого ADR
    /// </summary>
    public int TargetAdrNumber { get; set; }

    /// <summary>
    /// Имя указываемого ADR
    /// </summary>
    public string TargetAdrName { get; set; } = string.Empty;
}
