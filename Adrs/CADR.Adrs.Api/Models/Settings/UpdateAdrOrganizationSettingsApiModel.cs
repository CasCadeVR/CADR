namespace CADR.Adrs.Api.Models.Settings;

/// <summary>
/// API Модель обновления настроек ADR организации
/// </summary>
public class UpdateAdrOrganizationSettingsApiModel : AdrOrganizationSettingsApiModel
{
    /// <summary>
    /// Идентификатор пользователя, обновляющий настройки ADR организации
    /// </summary>
    public Guid UserId { get; set; }
}
