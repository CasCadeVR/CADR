using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Позиция компонента боковой панели
/// </summary>
public enum OffcanvasPosition
{
    /// <summary>Слева</summary>
    [CssClass("offcanvas-start")]
    Left,

    /// <summary>Сверху</summary>
    [CssClass("offcanvas-top")]
    Top,

    /// <summary>Снизу</summary>
    [CssClass("offcanvas-bottom")]
    Bottom,

    /// <summary>Справа</summary>
    [CssClass("offcanvas-end")]
    Right,
}
