using CADR.Adrs.Services.Contracts.Models.Enums;

namespace CADR.Adrs.Services.Contracts.Models.Adrs;

/// <summary>
/// Модель смена статуса ADR
/// </summary>
public class ChangeAdrStatusModel
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
    public AdrStatus Status { get; init; }
}
