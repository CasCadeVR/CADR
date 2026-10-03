using CADR.Api.Client;

namespace CADR.Portal.Contracts.Interfaces;

/// <summary>
/// Поставщик недавних организаций пользователя
/// </summary>
public interface IRecentOrganizationsProvider
{
    /// <summary>
    /// Проверяет актуальность списка недавних организаций
    /// </summary>
    void CheckOrganizations(Guid organizationId);

    /// <summary>
    /// Возвращает список недавних организаций
    /// </summary>
    /// <returns>
    /// Возвращает список недавних организаций пользователя, если он авторизирован,
    /// иначе вернёт <c>null</c>
    /// </returns>
    Task<List<OrganizationApiModel>?> GetOrganizationsAsync(CancellationToken token);
}
