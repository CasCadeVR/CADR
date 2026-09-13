using CADR.Adrs.Entities;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="AdrOrganizationSettings"/>
/// </summary>
public interface IAdrOrganizationSettingsReadRepository
{
    /// <summary>
    /// Получает настройки ADR по идентификатору организации
    /// </summary>
    Task<AdrOrganizationSettings?> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken);
}
