using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Flex;

/// <summary>
/// Выравнивание flex-элементов по главной оси (ось x для начала,
/// ось y, если flex-direction: столбец)
/// </summary>
public enum FlexJustify
{
    /// <summary>Начало</summary>
    [CssClass("justify-content-start")]
    Start,

    /// <summary>Конец</summary>
    [CssClass("justify-content-end")]
    End,

    /// <summary>Центр</summary>
    [CssClass("justify-content-center")]
    Center,

    /// <summary>Между</summary>
    [CssClass("justify-content-between")]
    Between,

    /// <summary>Вокруг</summary>
    [CssClass("justify-content-around")]
    Around,

    /// <summary>Равномерно</summary>
    [CssClass("justify-content-evenly")]
    Evenly,

    /// <summary>Начало для <see langword="small"/></summary>
    [CssClass("justify-content-sm-start")]
    StartSmall,

    /// <summary>Конец для <see langword="small"/></summary>
    [CssClass("justify-content-sm-end")]
    EndSmall,

    /// <summary>Центр для <see langword="small"/></summary>
    [CssClass("justify-content-sm-center")]
    CenterSmall,

    /// <summary>Между для <see langword="small"/></summary>
    [CssClass("justify-content-sm-between")]
    BetweenSmall,

    /// <summary>Вокруг для <see langword="small"/></summary>
    [CssClass("justify-content-sm-around")]
    AroundSmall,

    /// <summary>Равномерно для <see langword="small"/></summary>
    [CssClass("justify-content-sm-evenly")]
    EvenlySmall,

    /// <summary>Начало для <see langword="medium"/></summary>
    [CssClass("justify-content-md-start")]
    StartMedium,

    /// <summary>Конец для <see langword="medium"/></summary>
    [CssClass("justify-content-md-end")]
    EndMedium,

    /// <summary>Центр для <see langword="medium"/></summary>
    [CssClass("justify-content-md-center")]
    CenterMedium,

    /// <summary>Между для <see langword="medium"/></summary>
    [CssClass("justify-content-md-between")]
    BetweenMedium,

    /// <summary>Вокруг для <see langword="medium"/></summary>
    [CssClass("justify-content-md-around")]
    AroundMedium,

    /// <summary>Равномерно для <see langword="medium"/></summary>
    [CssClass("justify-content-md-evenly")]
    EvenlyMedium,

    /// <summary>Начало для <see langword="large"/></summary>
    [CssClass("justify-content-lg-start")]
    StartLarge,

    /// <summary>Конец для <see langword="large"/></summary>
    [CssClass("justify-content-lg-end")]
    EndLarge,

    /// <summary>Центр для <see langword="large"/></summary>
    [CssClass("justify-content-lg-center")]
    CenterLarge,

    /// <summary>Между для <see langword="large"/></summary>
    [CssClass("justify-content-lg-between")]
    BetweenLarge,

    /// <summary>Вокруг для <see langword="large"/></summary>
    [CssClass("justify-content-lg-around")]
    AroundLarge,

    /// <summary>Равномерно для <see langword="large"/></summary>
    [CssClass("justify-content-lg-evenly")]
    EvenlyLarge,

    /// <summary>Начало для <see langword="extra large"/></summary>
    [CssClass("justify-content-start")]
    StartExtraLarge,

    /// <summary>Конец для <see langword="extra large"/></summary>
    [CssClass("justify-content-xl-end")]
    EndExtraLarge,

    /// <summary>Центр для <see langword="extra large"/></summary>
    [CssClass("justify-content-xl-center")]
    CenterExtraLarge,

    /// <summary>Между для <see langword="extra large"/></summary>
    [CssClass("justify-content-xl-between")]
    BetweenExtraLarge,

    /// <summary>Вокруг для <see langword="extra large"/></summary>
    [CssClass("justify-content-xl-around")]
    AroundExtraLarge,

    /// <summary>Равномерно для <see langword="extra large"/></summary>
    [CssClass("justify-content-xl-evenly")]
    EvenlyExtraLarge,

    /// <summary>Начало для <see langword="extra extra large"/></summary>
    [CssClass("justify-content-xxl-start")]
    StartExtraExtraLarge,

    /// <summary>Конец для <see langword="extra extra large"/></summary>
    [CssClass("justify-content-xxl-end")]
    EndExtraExtraLarge,

    /// <summary>Центр для <see langword="extra extra large"/></summary>
    [CssClass("justify-content-xxl-center")]
    CenterExtraExtraLarge,

    /// <summary>Между для <see langword="extra extra large"/></summary>
    [CssClass("justify-content-xxl-between")]
    BetweenExtraExtraLarge,

    /// <summary>Вокруг для <see langword="extra extra large"/></summary>
    [CssClass("justify-content-xxl-around")]
    AroundExtraExtraLarge,

    /// <summary>Равномерно для <see langword="extra extra large"/></summary>
    [CssClass("justify-content-xxl-evenly")]
    EvenlyExtraExtraLarge,
}
