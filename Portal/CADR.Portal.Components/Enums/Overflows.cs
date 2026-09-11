using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Настройки переполнения содержимым элемента
/// </summary>
public enum Overflows
{
    /// <summary>Auto</summary>
    [CssClass("overflow-auto")]
    Auto,

    /// <summary>Hidden</summary>
    [CssClass("overflow-hidden")]
    Hidden,

    /// <summary>Visible</summary>
    [CssClass("overflow-visible")]
    Visible,

    /// <summary>Scroll</summary>
    [CssClass("overflow-scroll")]
    Scroll,

    /// <summary>Auto</summary>
    [CssClass("overflow-x-auto")]
    AutoX,

    /// <summary>Hidden</summary>
    [CssClass("overflow-x-hidden")]
    HiddenX,

    /// <summary>Visible</summary>
    [CssClass("overflow-x-visible")]
    VisibleX,

    /// <summary>Scroll</summary>
    [CssClass("overflow-x-scroll")]
    ScrollX,

    /// <summary>Auto</summary>
    [CssClass("overflow-y-auto")]
    AutoY,

    /// <summary>Hidden</summary>
    [CssClass("overflow-y-hidden")]
    HiddenY,

    /// <summary>Visible</summary>
    [CssClass("overflow-y-visible")]
    VisibleY,

    /// <summary>Scroll</summary>
    [CssClass("overflow-y-scroll")]
    ScrollY,
}
