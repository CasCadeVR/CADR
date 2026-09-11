using CADR.Administrations.Services.Contracts.Models.Enums;

namespace CADR.Administrations.Services.Contracts.Models.Organizations;

/// <summary>
/// Модель изменения роли пользователя
/// </summary>
public class ChangeUserRoleModel
{
    /// <summary>
    /// Идентификатор пользователя, выполнившего запрос
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Идентификатор пользователя, которому нужно поменять роль
    /// </summary>
    public Guid UserToUpdateId { get; set; }

    /// <summary>
    /// Роль, которую нужно установить пользователю
    /// </summary>
    public UserRole Role { get; set; }
}
