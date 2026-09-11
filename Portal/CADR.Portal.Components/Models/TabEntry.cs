using CADR.Portal.Components.Enums;

namespace CADR.Portal.Components.Models;

/// <summary>
/// Метаданные таба
/// </summary>
public class TabEntry<T>
    where T : struct, Enum
{
    /// <summary>
    /// Отображаемый текст
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Ссылка
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Иконка
    /// </summary>
    public IconTypes Icon { get; set; }

    /// <summary>
    /// Значение таба
    /// </summary>
    public T EnumValue { get; set; }
}
