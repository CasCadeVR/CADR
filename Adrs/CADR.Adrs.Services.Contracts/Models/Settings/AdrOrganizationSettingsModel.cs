using CADR.Adrs.Services.Contracts.Models.Enums;

namespace CADR.Adrs.Services.Contracts.Models.Settings;

/// <summary>
/// Модель настроек ADR организации
/// </summary>
public class AdrOrganizationSettingsModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Сколько нужно лайков для того чтобы автоматически сделать ADR утверждённой (<see cref="AdrStatus.Approved"/>)
    /// 0 - ADR утверждённы по умолчанию
    /// </summary>
    public int LikesRequiredForApproval { get; set; } = 1;
}
