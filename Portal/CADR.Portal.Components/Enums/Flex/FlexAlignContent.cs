using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Flex;

/// <summary>
/// Выравнивание адаптивных элементов по поперечной оси.
/// </summary>
public enum FlexAlignContent
{
    /// <summary>Начало</summary>
    [CssClass("align-content-start")]
    Start,

    /// <summary>Конец</summary>
    [CssClass("align-content-end")]
    End,

    /// <summary>Центр</summary>
    [CssClass("align-content-center")]
    Center,

    /// <summary>Между</summary>
    [CssClass("align-content-around")]
    Around,

    /// <summary>Растянуть</summary>
    [CssClass("align-content-stretch")]
    Stretch,

    /// <summary>Начало для <see langword="small"/></summary>
    [CssClass("align-content-sm-start")]
    StartSmall,

    /// <summary>Конец для <see langword="small"/></summary>
    [CssClass("align-content-sm-end")]
    EndSmall,

    /// <summary>Центр для <see langword="small"/></summary>
    [CssClass("align-content-sm-center")]
    CenterSmall,

    /// <summary>Между для <see langword="small"/></summary>
    [CssClass("align-content-sm-around")]
    AroundSmall,

    /// <summary>Растянуть для <see langword="small"/></summary>
    [CssClass("align-content-sm-stretch")]
    StretchSmall,

    /// <summary>Начало для <see langword="medium"/></summary>
    [CssClass("align-content-md-start")]
    StartMedium,

    /// <summary>Конец для <see langword="medium"/></summary>
    [CssClass("align-content-md-end")]
    EndMedium,

    /// <summary>Центр для <see langword="medium"/></summary>
    [CssClass("align-content-md-center")]
    CenterMedium,

    /// <summary>Между для <see langword="medium"/></summary>
    [CssClass("align-content-md-around")]
    AroundMedium,

    /// <summary>Растянуть для <see langword="medium"/></summary>
    [CssClass("align-content-md-stretch")]
    StretchMedium,

    /// <summary>Начало для <see langword="large"/></summary>
    [CssClass("align-content-lg-start")]
    StartLarge,

    /// <summary>Конец для <see langword="large"/></summary>
    [CssClass("align-content-lg-end")]
    EndLarge,

    /// <summary>Центр для <see langword="large"/></summary>
    [CssClass("align-content-lg-center")]
    CenterLarge,

    /// <summary>Между для <see langword="large"/></summary>
    [CssClass("align-content-lg-around")]
    AroundLarge,

    /// <summary>Растянуть для <see langword="large"/></summary>
    [CssClass("align-content-lg-stretch")]
    StretchLarge,

    /// <summary>Начало для <see langword="extra large"/></summary>
    [CssClass("align-content-start")]
    StartExtraLarge,

    /// <summary>Конец для <see langword="extra large"/></summary>
    [CssClass("align-content-xl-end")]
    EndExtraLarge,

    /// <summary>Центр для <see langword="extra large"/></summary>
    [CssClass("align-content-xl-center")]
    CenterExtraLarge,

    /// <summary>Между для <see langword="extra large"/></summary>
    [CssClass("align-content-xl-around")]
    AroundExtraLarge,

    /// <summary>Растянуть для <see langword="extra large"/></summary>
    [CssClass("align-content-xl-stretch")]
    StretchExtraLarge,

    /// <summary>Начало для <see langword="extra extra large"/></summary>
    [CssClass("align-content-xxl-start")]
    StartExtraExtraLarge,

    /// <summary>Конец для <see langword="extra extra large"/></summary>
    [CssClass("align-content-xxl-end")]
    EndExtraExtraLarge,

    /// <summary>Центр для <see langword="extra extra large"/></summary>
    [CssClass("align-content-xxl-center")]
    CenterExtraExtraLarge,

    /// <summary>Между для <see langword="extra extra large"/></summary>
    [CssClass("align-content-xxl-around")]
    AroundExtraExtraLarge,

    /// <summary>Растянуть для <see langword="extra extra large"/></summary>
    [CssClass("align-content-xxl-stretch")]
    StretchExtraExtraLarge,
}
