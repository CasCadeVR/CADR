using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Размер элемента
/// </summary>
public enum InputGroupSize
{
    /// <summary>Элемент большого размера</summary>
    [CssClass("input-group-lg")]
    Large,

    /// <summary>Элемент стандартного размера</summary>
    [CssClass("")]
    Default,

    /// <summary>Элемент меньшего размера</summary>
    [CssClass("input-group-sm")]
    Small,
}
