using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Text;

/// <summary>
/// Выравнивание текста
/// </summary>
[Flags]
public enum TextAlignments
{
    /// <summary>По левому краю</summary>
    [CssClass("text-start")]
    Left = 1,

    /// <summary>По центру</summary>
    [CssClass("text-center")]
    Center = 1 << 1,

    /// <summary>По правому краю</summary>
    [CssClass("text-end")]
    Right = 1 << 2,

    /// <summary>По левому краю для <see langword="small"/></summary>
    [CssClass("text-sm-start")]
    LeftSmall = 1 << 3,

    /// <summary>По центру для <see langword="small"/></summary>
    [CssClass("text-sm-center")]
    CenterSmall = 1 << 4,

    /// <summary>По правому краю для <see langword="small"/></summary>
    [CssClass("text-sm-end")]
    RightSmall = 1 << 5,

    /// <summary>По левому краю для <see langword="medium"/></summary>
    [CssClass("text-md-start")]
    LeftMedium = 1 << 6,

    /// <summary>По центру для <see langword="medium"/></summary>
    [CssClass("text-md-center")]
    CenterMedium = 1 << 7,

    /// <summary>По правому краю для <see langword="medium"/></summary>
    [CssClass("text-md-end")]
    RightMedium = 1 << 8,

    /// <summary>По левому краю для <see langword="large"/></summary>
    [CssClass("text-lg-start")]
    LeftLarge = 1 << 9,

    /// <summary>По центру для <see langword="large"/></summary>
    [CssClass("text-lg-center")]
    CenterLarge = 1 << 10,

    /// <summary>По правому краю для <see langword="large"/></summary>
    [CssClass("text-lg-end")]
    RightLarge = 1 << 11,

    /// <summary>По левому краю для <see langword="extra large"/></summary>
    [CssClass("text-xl-start")]
    LeftExtraLarge = 1 << 12,

    /// <summary>По центру для <see langword="extra large"/></summary>
    [CssClass("text-xl-center")]
    CenterExtraLarge = 1 << 13,

    /// <summary>По правому краю для <see langword="extra large"/></summary>
    [CssClass("text-xl-end")]
    RightExtraLarge = 1 << 14,

    /// <summary>По левому краю для <see langword="extra extra large"/></summary>
    [CssClass("text-xxl-start")]
    LeftExtraExtraLarge = 1 << 15,

    /// <summary>По центру для <see langword="extra extra large"/></summary>
    [CssClass("text-xxl-center")]
    CenterExtraExtraLarge = 1 << 16,

    /// <summary>По правому краю для <see langword="extra extra large"/></summary>
    [CssClass("text-xxl-end")]
    RightExtraExtraLarge = 1 << 17,
}
