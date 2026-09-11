using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Flex;

/// <summary>
/// Перенос элементов
/// </summary>
[Flags]
public enum FlexWraps
{
    /// <summary>Не переносить</summary>
    [CssClass("flex-nowrap")]
    Nowrap = 1,

    /// <summary>Переносить</summary>
    [CssClass("flex-wrap")]
    Wrap = 1 << 1,

    /// <summary>Переносить в обратном порядке</summary>
    [CssClass("flex-wrap-reverse")]
    WrapReverse = 1 << 2,

    /// <summary>Не переносить для <see langword="small"/></summary>
    [CssClass("flex-sm-nowrap")]
    NowrapSmall = 1 << 3,

    /// <summary>Переносить для <see langword="small"/></summary>
    [CssClass("flex-sm-wrap")]
    WrapSmall = 1 << 4,

    /// <summary>Переносить в обратном порядке для <see langword="small"/></summary>
    [CssClass("flex-sm-wrap-reverse")]
    WrapReverseSmall = 1 << 5,

    /// <summary>Не переносить для <see langword="medium"/></summary>
    [CssClass("flex-md-nowrap")]
    NowrapMedium = 1 << 6,

    /// <summary>Переносить для <see langword="medium"/></summary>
    [CssClass("flex-md-wrap")]
    WrapMedium = 1 << 7,

    /// <summary>Переносить в обратном порядке для <see langword="medium"/></summary>
    [CssClass("flex-md-wrap-reverse")]
    WrapReverseMedium = 1 << 8,

    /// <summary>Не переносить для <see langword="large"/></summary>
    [CssClass("flex-lg-nowrap")]
    NowrapLarge = 1 << 9,

    /// <summary>Переносить для <see langword="large"/></summary>
    [CssClass("flex-lg-wrap")]
    WrapLarge = 1 << 10,

    /// <summary>Переносить в обратном порядке для <see langword="large"/></summary>
    [CssClass("flex-lg-wrap-reverse")]
    WrapReverseLarge = 1 << 11,

    /// <summary>Не переносить для <see langword="extra large"/></summary>
    [CssClass("flex-xl-nowrap")]
    NowrapExtraLarge = 1 << 12,

    /// <summary>Переносить для <see langword="extra large"/></summary>
    [CssClass("flex-xl-wrap")]
    WrapExtraLarge = 1 << 13,

    /// <summary>Переносить в обратном порядке для <see langword="extra large"/></summary>
    [CssClass("flex-xl-wrap-reverse")]
    WrapReverseExtraLarge = 1 << 14,

    /// <summary>Не переносить для <see langword="extra extra large"/></summary>
    [CssClass("flex-xxl-nowrap")]
    NowrapExtraExtraLarge = 1 << 15,

    /// <summary>Переносить для <see langword="extra extra large"/></summary>
    [CssClass("flex-xxl-wrap")]
    WrapExtraExtraLarge = 1 << 16,

    /// <summary>Переносить в обратном порядке для <see langword="extra extra large"/></summary>
    [CssClass("flex-xxl-wrap-reverse")]
    WrapReverseExtraExtraLarge = 1 << 17,
}
