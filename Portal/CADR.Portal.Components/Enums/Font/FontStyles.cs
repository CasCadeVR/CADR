using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Font;

/// <summary>
/// Стили шрифта
/// </summary>
public enum FontStyles
{
    /// <summary>Обычный</summary>
    [CssClass("fst-normal")]
    Normal,

    /// <summary>Наклонный</summary>
    [CssClass("fst-italic")]
    Italic,

    /// <summary>Жирный</summary>
    [CssClass("fw-bold")]
    Bold,

    /// <summary>Сильно жирный</summary>
    [CssClass("fw-bolder")]
    Bolder,

    /// <summary>Слабо жирный</summary>
    [CssClass("fw-normal")]
    BoldNormal,

    /// <summary>Легковесный</summary>
    [CssClass("fw-light")]
    Light,

    /// <summary>Более легковесный</summary>
    [CssClass("fw-lighter")]
    Lighter,
}
