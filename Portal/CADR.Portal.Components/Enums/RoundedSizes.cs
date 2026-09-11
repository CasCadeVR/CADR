using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Размеры скругления
/// </summary>
public enum RoundedSizes
{
    /// <summary>Без скругления</summary>
    [CssClass("rounded-0")]
    Rounded0 = 0,

    /// <summary>Минимальное скругление</summary>
    [CssClass("rounded-1")]
    Rounded1 = 1,

    /// <summary>Среднее скругление</summary>
    [CssClass("rounded-2")]
    Rounded2 = 2,

    /// <summary>Максимальное скругление</summary>
    [CssClass("rounded-3")]
    Rounded3 = 3,
}
