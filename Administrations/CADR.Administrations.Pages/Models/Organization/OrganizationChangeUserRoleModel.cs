using CADR.Api.Client;

namespace CADR.Administrations.Pages.Models.Organization;

/// <summary>
/// Модель изменения роли пользователя
/// </summary>
public class OrganizationChangeUserRoleModel
{
    /// <inheritdoc cref="UserOrganizationApiModel"/>
    public UserOrganizationApiModel? UserOrganization { get; set; }

    /// <inheritdoc cref="UserRoleApi"/>
    public UserRoleApi NewRole { get; set; }
}
