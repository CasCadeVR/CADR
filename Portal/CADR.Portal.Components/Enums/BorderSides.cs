using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Стороны компонента
/// </summary>
public enum BorderSides
{
    /// <summary>
    /// Лево
    /// </summary>
    [CssClass("left")]
    Left,

    /// <summary>
    /// Верх
    /// </summary>
    [CssClass("top")]
    Top,

    /// <summary>
    /// Право
    /// </summary>
    [CssClass("right")]
    Right,

    /// <summary>
    /// Низ
    /// </summary>
    [CssClass("bottom")]
    Bottom
}
