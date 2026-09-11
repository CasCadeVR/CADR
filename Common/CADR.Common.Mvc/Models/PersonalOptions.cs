using System.Security.Claims;

namespace CADR.Common.Mvc.Models;

/// <summary>
/// Класс представляет информацию, необходимую для управления поведением
/// при создании <see cref="Claim"/>
/// </summary>
public class PersonalOptions
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Identifier { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Адрес электронной почты
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Имя входа
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Отметка безопасности
    /// </summary>
    public string SecurityStamp { get; set; } = string.Empty;

    /// <summary>
    /// Пользовательские параметры
    /// </summary>
    public IReadOnlyDictionary<string, string> Params { get; set; } = new Dictionary<string, string>();
}
