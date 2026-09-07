using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Text;

/// <summary>
/// Стили подчёркивания
/// </summary>
public enum TextDecorations
{
    /// <summary>Подчёркнутый</summary>
    [CssClass("text-decoration-underline")]
    Underline,

    /// <summary>Зачёркнутый</summary>
    [CssClass("text-decoration-line-through")]
    LineThrough,

    /// <summary>Без подчёркивания</summary>
    [CssClass("text-decoration-none")]
    None,
}
