using CADR.Administrations.Entities.Views;

namespace CADR.Administrations.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="OrganizationMemberView"/>
/// </summary>
public interface IOrganizationMemberViewReadRepository
{
    /// <summary>
    /// Получает список <see cref="OrganizationMemberView"/> по идентификатору организации
    /// </summary>
    Task<IReadOnlyCollection<OrganizationMemberView>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken ct);
}
