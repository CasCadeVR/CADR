using CADR.Portal.Components.Enums;

namespace CADR.Portal.Components.Models;

/// <summary>
/// Элемент хлебных крошек
/// </summary>
public class LinkedItem
{
    /// <summary>
    /// Заголовок элемента
    /// </summary>
    public string Caption { get; set; } = string.Empty;

    /// <summary>
    /// Ссылка на элемент
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <inheritdoc cref="IconTypes"/>
    public IconTypes? IconType { get; set; }
}
