using CADR.Portal.Components.Enums;

namespace CADR.Portal.Components.Models;

/// <summary>
/// Элемент группы кнопок
/// </summary>
public class ButtonGroupItem<T>
{
    /// <summary>Цвет кнопки</summary>
    public ThemeColors? Color { get; set; }

    /// <summary>Текст кнопки</summary>
    public string Text { get; set; } = string.Empty;

    /// <inheritdoc cref="IconTypes"/>
    public IconTypes? IconType { get; set; }

    /// <summary>
    /// Значение элемента
    /// </summary>
    public T Value { get; set; } = default!;

    /// <summary>
    /// Ссылка
    /// </summary>
    public string? Uri { get; set; }
}
