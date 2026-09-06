using CADR.Administrations.Services.Contracts.Models.Organizations;

namespace CADR.Administrations.Services.Contracts.Interfaces;

/// <summary>
/// Управление организациями
/// </summary>
public interface IOrganizationManager
{
    /// <summary>
    /// Создаёт новую организацию
    /// </summary>
    Task<OrganizationModel> CreateAsync(CreateOrganizationModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список организаций для указанного идентификатора пользователя
    /// </summary>
    Task<IEnumerable<OrganizationModel>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает организацию по идентификатору для указанного идентификатора пользователя
    /// </summary>
    Task<OrganizationModel> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Редактирует существующую организацию
    /// </summary>
    Task<OrganizationModel> UpdateAsync(UpdateOrganizationModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет существующую организацию
    /// </summary>
    Task DeleteAsync(DeleteOrganizationModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список пользователей организации пользователя
    /// </summary>
    Task<IEnumerable<UserOrganizationModel>> GetUsersByOrganizationIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Задаёт новую роль пользователю
    /// </summary>
    Task ChangeUserRoleAsync(ChangeUserRoleModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет пользователя из организации
    /// </summary>
    Task DeleteUserAsync(DeleteUserOrganizationModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает одного пользователя в организации
    /// </summary>
    Task<UserOrganizationModel> GetUserOrganizationAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken);
}
