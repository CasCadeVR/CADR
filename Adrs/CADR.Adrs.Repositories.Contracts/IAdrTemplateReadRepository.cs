using CADR.Adrs.Entities;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="AdrTemplate"/>
/// </summary>
public interface IAdrTemplateReadRepository
{
    /// <summary>
    /// Получает активную <see cref="AdrTemplate"/> по идентификатору
    /// </summary>
    Task<AdrTemplate?> GetActiveByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех шаблонов ADR организации
    /// </summary>
    Task<IReadOnlyCollection<AdrTemplate>> GetAvailableForOrganizationAsync(Guid organizationId, CancellationToken cancellationToken);

    /// <summary>
    /// Существует ли шаблон ADR с таким именем
    /// </summary>
    Task<bool> IsActiveNameExistsAsync(Guid organizationId, string name, CancellationToken cancellationToken);
}
