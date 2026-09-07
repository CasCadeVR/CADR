using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Flex;

/// <summary>
/// Выравнивание flex-элементов по поперечной оси (ось Y для начала,
/// ось x, если flex-direction: столбец).
/// </summary>
public enum FlexAlign
{
    /// <summary>Начало</summary>
    [CssClass("align-items-start")]
    Start,

    /// <summary>Конец</summary>
    [CssClass("align-items-end")]
    End,

    /// <summary>Центр</summary>
    [CssClass("align-items-center")]
    Center,

    /// <summary>Между</summary>
    [CssClass("align-items-baseline")]
    Baseline,

    /// <summary>Растянуть</summary>
    [CssClass("align-items-stretch")]
    Stretch,

    /// <summary>Начало для <see langword="small"/></summary>
    [CssClass("align-items-sm-start")]
    StartSmall,

    /// <summary>Конец для <see langword="small"/></summary>
    [CssClass("align-items-sm-end")]
    EndSmall,

    /// <summary>Центр для <see langword="small"/></summary>
    [CssClass("align-items-sm-center")]
    CenterSmall,

    /// <summary>Между для <see langword="small"/></summary>
    [CssClass("align-items-sm-baseline")]
    BaselineSmall,

    /// <summary>Растянуть для <see langword="small"/></summary>
    [CssClass("align-items-sm-stretch")]
    StretchSmall,

    /// <summary>Начало для <see langword="medium"/></summary>
    [CssClass("align-items-md-start")]
    StartMedium,

    /// <summary>Конец для <see langword="medium"/></summary>
    [CssClass("align-items-md-end")]
    EndMedium,

    /// <summary>Центр для <see langword="medium"/></summary>
    [CssClass("align-items-md-center")]
    CenterMedium,

    /// <summary>Между для <see langword="medium"/></summary>
    [CssClass("align-items-md-baseline")]
    BaselineMedium,

    /// <summary>Растянуть для <see langword="medium"/></summary>
    [CssClass("align-items-md-stretch")]
    StretchMedium,

    /// <summary>Начало для <see langword="large"/></summary>
    [CssClass("align-items-lg-start")]
    StartLarge,

    /// <summary>Конец для <see langword="large"/></summary>
    [CssClass("align-items-lg-end")]
    EndLarge,

    /// <summary>Центр для <see langword="large"/></summary>
    [CssClass("align-items-lg-center")]
    CenterLarge,

    /// <summary>Между для <see langword="large"/></summary>
    [CssClass("align-items-lg-baseline")]
    BaselineLarge,

    /// <summary>Растянуть для <see langword="large"/></summary>
    [CssClass("align-items-lg-stretch")]
    StretchLarge,

    /// <summary>Начало для <see langword="extra large"/></summary>
    [CssClass("align-items-start")]
    StartExtraLarge,

    /// <summary>Конец для <see langword="extra large"/></summary>
    [CssClass("align-items-xl-end")]
    EndExtraLarge,

    /// <summary>Центр для <see langword="extra large"/></summary>
    [CssClass("align-items-xl-center")]
    CenterExtraLarge,

    /// <summary>Между для <see langword="extra large"/></summary>
    [CssClass("align-items-xl-baseline")]
    BaselineExtraLarge,

    /// <summary>Растянуть для <see langword="extra large"/></summary>
    [CssClass("align-items-xl-stretch")]
    StretchExtraLarge,

    /// <summary>Начало для <see langword="extra extra large"/></summary>
    [CssClass("align-items-xxl-start")]
    StartExtraExtraLarge,

    /// <summary>Конец для <see langword="extra extra large"/></summary>
    [CssClass("align-items-xxl-end")]
    EndExtraExtraLarge,

    /// <summary>Центр для <see langword="extra extra large"/></summary>
    [CssClass("align-items-xxl-center")]
    CenterExtraExtraLarge,

    /// <summary>Между для <see langword="extra extra large"/></summary>
    [CssClass("align-items-xxl-baseline")]
    BaselineExtraExtraLarge,

    /// <summary>Растянуть для <see langword="extra extra large"/></summary>
    [CssClass("align-items-xxl-stretch")]
    StretchExtraExtraLarge,
}
