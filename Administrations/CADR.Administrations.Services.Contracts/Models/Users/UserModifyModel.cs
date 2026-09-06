namespace CADR.Administrations.Services.Contracts.Models.User;

/// <summary>
/// Модель изменения пользователя
/// </summary>
public class UserModifyModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Адрес электронной почты
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
