using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Flex;

/// <summary>
/// Направление адаптивной разметки
/// </summary>
public enum FlexDirections
{
    /// <summary>Адаптивный макет в направлении строки</summary>
    [CssClass("flex-row")]
    Row,

    /// <summary>Адаптивный макет в обратном направлении строки</summary>
    [CssClass("flex-row-reverse")]
    RowReverse,

    /// <summary>Адаптивный макет в направлении столбца</summary>
    [CssClass("flex-column")]
    Column,

    /// <summary>Адаптивный макет в обратном направлении столбца</summary>
    [CssClass("flex-column-reverse")]
    ColumnReverse,

    /// <summary>Адаптивный макет в направлении строки для <see langword="small"/></summary>
    [CssClass("flex-sm-row")]
    RowSmall,

    /// <summary>Адаптивный макет в обратном направлении строки для <see langword="small"/></summary>
    [CssClass("flex-sm-row-reverse")]
    RowReverseSmall,

    /// <summary>Адаптивный макет в направлении столбца для <see langword="small"/></summary>
    [CssClass("flex-sm-column")]
    ColumnSmall,

    /// <summary>Адаптивный макет в обратном направлении столбца для <see langword="small"/></summary>
    [CssClass("flex-sm-column-reverse")]
    ColumnReverseSmall,

    /// <summary>Адаптивный макет в направлении строки для <see langword="medium"/></summary>
    [CssClass("flex-md-row")]
    RowMedium,

    /// <summary>Адаптивный макет в обратном направлении строки для <see langword="medium"/></summary>
    [CssClass("flex-md-row-reverse")]
    RowReverseMedium,

    /// <summary>Адаптивный макет в направлении столбца для <see langword="medium"/></summary>
    [CssClass("flex-md-column")]
    ColumnMedium,

    /// <summary>Адаптивный макет в обратном направлении столбца для <see langword="medium"/></summary>
    [CssClass("flex-md-column-reverse")]
    ColumnReverseMedium,

    /// <summary>Адаптивный макет в направлении строки для <see langword="large"/></summary>
    [CssClass("flex-lg-row")]
    RowLarge,

    /// <summary>Адаптивный макет в обратном направлении строки для <see langword="large"/></summary>
    [CssClass("flex-lg-row-reverse")]
    RowReverseLarge,

    /// <summary>Адаптивный макет в направлении столбца для <see langword="large"/></summary>
    [CssClass("flex-lg-column")]
    ColumnLarge,

    /// <summary>Адаптивный макет в обратном направлении столбца для <see langword="large"/></summary>
    [CssClass("flex-lg-column-reverse")]
    ColumnReverseLarge,

    /// <summary>Адаптивный макет в направлении строки для <see langword="extra Large"/></summary>
    [CssClass("flex-xl-row")]
    RowExtraLarge,

    /// <summary>Адаптивный макет в обратном направлении строки для <see langword="extra Large"/></summary>
    [CssClass("flex-xl-row-reverse")]
    RowReverseExtraLarge,

    /// <summary>Адаптивный макет в направлении столбца для <see langword="extra Large"/></summary>
    [CssClass("flex-xl-column")]
    ColumnExtraLarge,

    /// <summary>Адаптивный макет в обратном направлении столбца для <see langword="extra Large"/></summary>
    [CssClass("flex-xl-column-reverse")]
    ColumnReverseExtraLarge,

    /// <summary>Адаптивный макет в направлении строки для <see langword="extra extra Large"/></summary>
    [CssClass("flex-xxl-row")]
    RowExtraExtraLarge,

    /// <summary>Адаптивный макет в обратном направлении строки для <see langword="extra extra Large"/></summary>
    [CssClass("flex-xxl-row-reverse")]
    RowReverseExtraExtraLarge,

    /// <summary>Адаптивный макет в направлении столбца для <see langword="extra extra Large"/></summary>
    [CssClass("flex-xxl-column")]
    ColumnExtraExtraLarge,

    /// <summary>Адаптивный макет в обратном направлении столбца для <see langword="extra extra Large"/></summary>
    [CssClass("flex-xxl-column-reverse")]
    ColumnReverseExtraExtraLarge,
}
