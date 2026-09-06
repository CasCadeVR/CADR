namespace CADR.Administrations.Api.Models.Enums;

/// <summary>
/// Роли пользователя
/// </summary>
public enum UserRoleApi
{
    /// <summary>
    /// Просмотр и создание ADR
    /// </summary>
    User = 0,

    /// <summary>
    /// Учётные записи, организации
    /// </summary>
    Admin = 1,
}
