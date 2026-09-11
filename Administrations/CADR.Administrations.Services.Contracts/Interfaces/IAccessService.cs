namespace CADR.Administrations.Services.Contracts.Interfaces;

/// <summary>
/// Интерфейс сервиса доступа
/// </summary>
public interface IAccessService
{
    /// <summary>
    /// Является ли пользователь администратором хотя бы в одной организации для указанного пользователя
    /// </summary>
    Task IsAdminForAnyUserOrganizationAsync(Guid userId, CancellationToken cancellationToken);
}
