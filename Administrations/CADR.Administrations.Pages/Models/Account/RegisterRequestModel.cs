using System.ComponentModel.DataAnnotations;

namespace CADR.Administrations.Pages.Models.Account;

/// <summary>
/// Модель запроса создания пользователя
/// </summary>
public class RegisterRequestModel
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
    [MinLength(6, ErrorMessage = "Пароль должен быть минимум 6 символов")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Пароль ещё раз
    /// </summary>
    [Required(ErrorMessage = "Укажите пароль ещё раз")]
    [Compare(nameof(Password), ErrorMessage = "Пароли должны совпадать")]
    public string PasswordAgain { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    [Required(ErrorMessage = "Укажите ваше имя")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Электронный адрес
    /// </summary>
    [Required(ErrorMessage = "Укажите email адрес")]
    [EmailAddress(ErrorMessage = "Укажите правильный формат")]
    public string Email { get; set; } = string.Empty;
}
