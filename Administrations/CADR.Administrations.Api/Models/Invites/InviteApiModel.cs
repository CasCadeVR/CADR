using CADR.Administrations.Api.Models.Enums;

namespace CADR.Administrations.Api.Models.Invites;

/// <summary>
/// Модель приглашения пользователя к организации
/// </summary>
public class InviteApiModel
{
    /// <summary>
    /// Почтовый адрес пользователя
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <inheritdoc cref="UserRoleApi"/>
    public UserRoleApi Role { get; set; }
}
