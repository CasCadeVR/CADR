using CADR.Administrations.Api.Models.Enums;

namespace CADR.Administrations.Api.Models.Organizations;

/// <summary>
/// Api модель организации
/// </summary>
public class OrganizationApiModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Роль запрашивающего пользователя
    /// </summary>
    public UserRoleApi RequestingUserRole { get; set; }
}
