using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Flex;

/// <summary>
/// Типы адаптивного макета
/// </summary>
public enum Flexes
{
    /// <summary>Адаптивный макет</summary>
    [CssClass("d-flex")]
    Flex,

    /// <summary>Адаптивный встроенный макет</summary>
    [CssClass("d-inline-flex")]
    FlexInline,

    /// <summary>Адаптивный макет для <see langword="small"/></summary>
    [CssClass("d-sm-flex")]
    FlexSmall,

    /// <summary>Адаптивный встроенный макет для <see langword="small"/></summary>
    [CssClass("d-sm-inline-flex")]
    FlexSmallInline,

    /// <summary>Адаптивный макет для <see langword="medium"/></summary>
    [CssClass("d-md-flex")]
    FlexMedium,

    /// <summary>Адаптивный встроенный макет для <see langword="medium"/></summary>
    [CssClass("d-md-inline-flex")]
    FlexMediumInline,

    /// <summary>Адаптивный макет для <see langword="large"/></summary>
    [CssClass("d-lg-flex")]
    FlexLarge,

    /// <summary>Адаптивный встроенный макет для <see langword="large"/></summary>
    [CssClass("d-lg-inline-flex")]
    FlexLargeInline,

    /// <summary>Адаптивный макет для <see langword="extra large"/></summary>
    [CssClass("d-xl-flex")]
    FlexExtraLarge,

    /// <summary>Адаптивный встроенный макет для <see langword="extra large"/></summary>
    [CssClass("d-xl-inline-flex")]
    FlexExtraLargeInline,

    /// <summary>Адаптивный макет для <see langword="extra extra large"/></summary>
    [CssClass("d-xxl-flex")]
    FlexExtraExtraLarge,

    /// <summary>Адаптивный встроенный макет для <see langword="extra extra large"/></summary>
    [CssClass("d-xxl-inline-flex")]
    FlexExtraExtraLargeInline,

    /// <summary>Автоматическое выравнивание со смещением вправо</summary>
    [CssClass("me-auto")]
    AutoRight,

    /// <summary>Автоматическое выравнивание со смещением влево</summary>
    [CssClass("ms-auto")]
    AutoLeft,
}
