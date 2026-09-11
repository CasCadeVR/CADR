using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Плавающий элемент
/// </summary>
[Flags]
public enum Floats
{
    /// <summary>Float left</summary>
    [CssClass("float-left")]
    Left = 1,

    /// <summary>Float right</summary>
    [CssClass("float-right")]
    Right = 1 << 1,

    /// <summary>Float none</summary>
    [CssClass("float-none")]
    None = 1 << 2,

    /// <summary>Float left для <see langword="small"/></summary>
    [CssClass("float-sm-left")]
    LeftSmall = 1 << 3,

    /// <summary>Float right для <see langword="small"/></summary>
    [CssClass("float-sm-right")]
    RightSmall = 1 << 4,

    /// <summary>Float none для <see langword="small"/></summary>
    [CssClass("float-sm-none")]
    NoneSmall = 1 << 5,

    /// <summary>Float left для <see langword="medium"/></summary>
    [CssClass("float-md-left")]
    LeftMedium = 1 << 6,

    /// <summary>Float right для <see langword="medium"/></summary>
    [CssClass("float-md-right")]
    RightMedium = 1 << 7,

    /// <summary>Float none для <see langword="medium"/></summary>
    [CssClass("float-md-none")]
    NoneMedium = 1 << 8,

    /// <summary>Float left для <see langword="large"/></summary>
    [CssClass("float-lg-left")]
    LeftLarge = 1 << 9,

    /// <summary>Float right для <see langword="large"/></summary>
    [CssClass("float-lg-right")]
    RightLarge = 1 << 10,

    /// <summary>Float none для <see langword="large"/></summary>
    [CssClass("float-lg-none")]
    NoneLarge = 1 << 11,

    /// <summary>Float left для <see langword="extra large"/></summary>
    [CssClass("float-xl-left")]
    LeftExtraLarge = 1 << 2,

    /// <summary>Float right для <see langword="extra large"/></summary>
    [CssClass("float-xl-right")]
    RightExtraLarge = 1 << 13,

    /// <summary>Float none для <see langword="extra large"/></summary>
    [CssClass("float-xl-none")]
    NoneExtraLarge = 1 << 14,

    /// <summary>Float left для <see langword="extra extra large"/></summary>
    [CssClass("float-xxl-left")]
    LeftExtraExtraLarge = 1 << 15,

    /// <summary>Float right для <see langword="extra extra large"/></summary>
    [CssClass("float-xxl-right")]
    RightExtraExtraLarge = 1 << 16,

    /// <summary>Float none для <see langword="extra extra large"/></summary>
    [CssClass("float-xxl-none")]
    NoneExtraExtraLarge = 1 << 17,
}
