using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Text;

/// <summary>
/// Трансформации текста
/// </summary>
public enum TextTransforms
{
    /// <summary>Нижний регистр</summary>
    [CssClass("text-lowercase")]
    Lowercase,

    /// <summary>Верхний регистр</summary>
    [CssClass("text-uppercase")]
    Uppercase,

    /// <summary>С большой буквы</summary>
    [CssClass("text-capitalize")]
    Capitalize
}
