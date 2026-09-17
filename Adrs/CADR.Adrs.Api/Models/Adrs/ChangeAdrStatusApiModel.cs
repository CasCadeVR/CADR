using CADR.Adrs.Api.Models.Enums;

namespace CADR.Adrs.Api.Models.Adrs;

/// <summary>
/// API Модель смена статуса ADR
/// </summary>
public class ChangeAdrStatusApiModel
{
    /// <summary>
    /// Идентификатор пользователя, сменяющий статус ADR
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Идентификатор ADR
    /// </summary>
    public Guid AdrId { get; init; }

    /// <summary>
    /// Стаутс ADR
    /// </summary>
    public AdrStatusApi Status { get; init; }
}
