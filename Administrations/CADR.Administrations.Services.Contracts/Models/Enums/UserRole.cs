namespace CADR.Administrations.Services.Contracts.Models.Enums;

/// <summary>
/// Роли пользователя
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Просмотр и комментирование ADR
    /// </summary>
    User = 0,

    /// <summary>
    /// Создание и редактирование ADR
    /// </summary>
    Architect = 1,

    /// <summary>
    /// Учётные записи, организации
    /// </summary>
    Admin = 2,
}
