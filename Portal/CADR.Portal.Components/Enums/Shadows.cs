using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Характеристики тени
/// </summary>
public enum Shadows
{
    /// <summary>Без тени</summary>
    [CssClass("shadow-none")]
    None,

    /// <summary>Небольшая тень</summary>
    [CssClass("shadow-sm")]
    Small,

    /// <summary>Обычная тень</summary>
    [CssClass("shadow")]
    Normal,

    /// <summary>Большая тень</summary>
    [CssClass("shadow-lg")]
    Large,
}
