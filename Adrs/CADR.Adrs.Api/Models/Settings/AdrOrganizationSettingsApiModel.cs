using CADR.Adrs.Api.Models.Enums;

namespace CADR.Adrs.Api.Models.Settings;

/// <summary>
/// API Модель настроек ADR организации
/// </summary>
public class AdrOrganizationSettingsApiModel
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
    /// Сколько нужно лайков для того чтобы автоматически сделать ADR утверждённой (<see cref="AdrStatusApi.Approved"/>)
    /// 0 - ADR утверждённы по умолчанию
    /// </summary>
    public int LikesRequiredForApproval { get; set; } = 1;
}
