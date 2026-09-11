using CADR.Administrations.Entities.Enums;

namespace CADR.Administrations.Repositories.Contracts.Extensions;

/// <summary>
/// Методы расширения для <see cref="IUserOrganizationReadRepository"/>
/// </summary>
public static class UserOrganizationReadRepositoryExtensions
{
    /// <summary>
    /// Проверить, является ли пользователь админом в организации, и бросить исключение, если нет
    /// </summary>
    public static async Task ThrowIfNotAdminAsync<T>(
        this IUserOrganizationReadRepository repository,
        Guid userId,
        Guid organizationId,
        CancellationToken cancellationToken)
        where T : Exception, new()
    {
        var user = await repository.GetByUserAndOrganizationIdAsync(userId, organizationId, cancellationToken);
        if (user?.Role is not Role.Admin)
        {
            throw new T();
        }
    }

    /// <summary>
    /// Проверить, является ли пользователь членом организации, и бросить исключение, если нет
    /// </summary>
    public static async Task ThrowIfNotMemberAsync<T>(
        this IUserOrganizationReadRepository repository,
        Guid userId,
        Guid organizationId,
        CancellationToken cancellationToken)
        where T : Exception, new()
    {
        var user = await repository.GetByUserAndOrganizationIdAsync(userId, organizationId, cancellationToken);
        if (user is null)
        {
            throw new T();
        }
    }
}
