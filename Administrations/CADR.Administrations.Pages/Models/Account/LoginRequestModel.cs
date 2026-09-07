using System.ComponentModel.DataAnnotations;

namespace CADR.Administrations.Pages.Models.Account;

/// <summary>
/// Модель запроса авторизации
/// </summary>
public class LoginRequestModel
{
    /// <summary>
    /// Имя входа
    /// </summary>
    [Required(ErrorMessage = "Укажите имя входа")]
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Пароль
    /// </summary>
    [Required(ErrorMessage = "Укажите пароль")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Запомнить меня
    /// </summary>
    public bool IsRemember { get; set; }
}
