using CADR.Administrations.Entities;
using CADR.Adrs.Entities.Enums;
using CADR.Context.Entities.Contracts.Models;

namespace CADR.Adrs.Entities;

/// <summary>
/// Настройки создания ADR в организации 
/// </summary>
public class AdrOrganizationSettings : BaseAuditEntity
{
    /// <summary>
    /// Сколько нужно лайков для того чтобы автоматически сделать ADR утверждённой (<see cref="AdrStatus.Approved"/>)
    /// 0 - ADR утверждённы по умолчанию
    /// </summary>
    public int LikesRequiredForApproval { get; set; } = 1;

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Навигационное свойство организации
    /// </summary>
    public Organization? Organization { get; set; }
}
