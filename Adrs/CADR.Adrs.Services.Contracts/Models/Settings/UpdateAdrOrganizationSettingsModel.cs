namespace CADR.Adrs.Services.Contracts.Models.Settings;

/// <summary>
/// Модель обновления настроек ADR организации
/// </summary>
public class UpdateAdrOrganizationSettingsModel : AdrOrganizationSettingsModel
{
    /// <summary>
    /// Идентификатор пользователя, обновляющий настройки ADR организации
    /// </summary>
    public Guid UserId { get; set; }
}
