namespace CADR.Administrations.Services.Contracts.Models.Enums;

/// <summary>
/// Роли пользователя
/// </summary>
public enum UserRole
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
