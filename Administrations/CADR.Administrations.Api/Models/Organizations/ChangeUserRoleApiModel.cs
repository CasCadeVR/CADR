using CADR.Administrations.Api.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CADR.Administrations.Api.Models.Organizations;

/// <summary>
/// Api модель изменения роли пользователя
/// </summary>
public class ChangeUserRoleApiModel
{
    /// <summary>
    /// Идентификатор организации
    /// </summary>
    [FromRoute(Name = "organizationId")]
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Идентификатор пользователя, которому нужно поменять роль
    /// </summary>
    [FromRoute(Name = "userToUpdateId")]
    public Guid UserToUpdateId { get; set; }

    /// <summary>
    /// Роль, которую нужно установить пользователю
    /// </summary>
    public UserRoleApi Role { get; set; }
}
