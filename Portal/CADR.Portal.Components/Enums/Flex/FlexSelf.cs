using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Flex;

/// <summary>
/// Выравнивание flex-элементов по поперечной оси (ось Y для начала,
/// ось x, если flex-direction: столбец).
/// </summary>
public enum FlexSelf
{
    /// <summary>Начало</summary>
    [CssClass("align-self-start")]
    Start,

    /// <summary>Конец</summary>
    [CssClass("align-self-end")]
    End,

    /// <summary>Центр</summary>
    [CssClass("align-self-center")]
    Center,

    /// <summary>Между</summary>
    [CssClass("align-self-baseline")]
    Baseline,

    /// <summary>Растянуть</summary>
    [CssClass("align-self-stretch")]
    Stretch,

    /// <summary>Начало для <see langword="small"/></summary>
    [CssClass("align-self-sm-start")]
    StartSmall,

    /// <summary>Конец для <see langword="small"/></summary>
    [CssClass("align-self-sm-end")]
    EndSmall,

    /// <summary>Центр для <see langword="small"/></summary>
    [CssClass("align-self-sm-center")]
    CenterSmall,

    /// <summary>Между для <see langword="small"/></summary>
    [CssClass("align-self-sm-baseline")]
    BaselineSmall,

    /// <summary>Растянуть для <see langword="small"/></summary>
    [CssClass("align-self-sm-stretch")]
    StretchSmall,

    /// <summary>Начало для <see langword="medium"/></summary>
    [CssClass("align-self-md-start")]
    StartMedium,

    /// <summary>Конец для <see langword="medium"/></summary>
    [CssClass("align-self-md-end")]
    EndMedium,

    /// <summary>Центр для <see langword="medium"/></summary>
    [CssClass("align-self-md-center")]
    CenterMedium,

    /// <summary>Между для <see langword="medium"/></summary>
    [CssClass("align-self-md-baseline")]
    BaselineMedium,

    /// <summary>Растянуть для <see langword="medium"/></summary>
    [CssClass("align-self-md-stretch")]
    StretchMedium,

    /// <summary>Начало для <see langword="large"/></summary>
    [CssClass("align-self-lg-start")]
    StartLarge,

    /// <summary>Конец для <see langword="large"/></summary>
    [CssClass("align-self-lg-end")]
    EndLarge,

    /// <summary>Центр для <see langword="large"/></summary>
    [CssClass("align-self-lg-center")]
    CenterLarge,

    /// <summary>Между для <see langword="large"/></summary>
    [CssClass("align-self-lg-baseline")]
    BaselineLarge,

    /// <summary>Растянуть для <see langword="large"/></summary>
    [CssClass("align-self-lg-stretch")]
    StretchLarge,

    /// <summary>Начало для <see langword="extra large"/></summary>
    [CssClass("align-self-start")]
    StartExtraLarge,

    /// <summary>Конец для <see langword="extra large"/></summary>
    [CssClass("align-self-xl-end")]
    EndExtraLarge,

    /// <summary>Центр для <see langword="extra large"/></summary>
    [CssClass("align-self-xl-center")]
    CenterExtraLarge,

    /// <summary>Между для <see langword="extra large"/></summary>
    [CssClass("align-self-xl-baseline")]
    BaselineExtraLarge,

    /// <summary>Растянуть для <see langword="extra large"/></summary>
    [CssClass("align-self-xl-stretch")]
    StretchExtraLarge,

    /// <summary>Начало для <see langword="extra extra large"/></summary>
    [CssClass("align-self-xxl-start")]
    StartExtraExtraLarge,

    /// <summary>Конец для <see langword="extra extra large"/></summary>
    [CssClass("align-self-xxl-end")]
    EndExtraExtraLarge,

    /// <summary>Центр для <see langword="extra extra large"/></summary>
    [CssClass("align-self-xxl-center")]
    CenterExtraExtraLarge,

    /// <summary>Между для <see langword="extra extra large"/></summary>
    [CssClass("align-self-xxl-baseline")]
    BaselineExtraExtraLarge,

    /// <summary>Растянуть для <see langword="extra extra large"/></summary>
    [CssClass("align-self-xxl-stretch")]
    StretchExtraExtraLarge,
}
