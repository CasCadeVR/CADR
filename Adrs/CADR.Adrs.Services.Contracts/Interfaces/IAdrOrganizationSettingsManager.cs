using CADR.Adrs.Services.Contracts.Models.Settings;

namespace CADR.Adrs.Services.Contracts.Interfaces;

/// <summary>
/// Управление настройками ADR организации
/// </summary>
public interface IAdrOrganizationSettingsManager
{
    /// <summary>
    /// Получает настройки ADR организации, при отсутствии возвращает значения по умолчанию
    /// </summary>
    Task<AdrOrganizationSettingsModel> GetByOrganizationIdAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет настройки ADR организации
    /// </summary>
    Task<AdrOrganizationSettingsModel> UpdateAsync(UpdateAdrOrganizationSettingsModel model, CancellationToken cancellationToken);
}
