using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Радиусы рамок
/// </summary>
public enum BorderRadius
{
    /// <summary>Скругления вокруг</summary>
    [CssClass("rounded")]
    Default,

    /// <summary>Скругления вверху</summary>
    [CssClass("rounded--top")]
    Top,

    /// <summary>Скругления справа</summary>
    [CssClass("rounded-end")]
    End,

    /// <summary>Скругления внизу</summary>
    [CssClass("rounded-bottom")]
    Bottom,

    /// <summary>Скругления слева</summary>
    [CssClass("rounded-start")]
    Start,

    /// <summary>Скругления по 50%</summary>
    [CssClass("rounded-circle")]
    Circle,

    /// <summary>Скругления pill</summary>
    [CssClass("rounded-pill")]
    Pill,
}
