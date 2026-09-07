using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Высота строчек в параграфе
/// </summary>
public enum LineHeights
{
    /// <summary>Минимальная высота строк</summary>
    [CssClass("lh-1")]
    Minimal,

    /// <summary>Небольшая высота строк</summary>
    [CssClass("lh-sm")]
    Small,

    /// <summary>Базовая высота строк</summary>
    [CssClass("lh-base")]
    Base,

    /// <summary>Большая высота строк</summary>
    [CssClass("lh-lg")]
    Large,
}
