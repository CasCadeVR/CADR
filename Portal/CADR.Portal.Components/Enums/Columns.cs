using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Характеристики столбца
/// </summary>
[Flags]
public enum Columns
{
    /// <summary>Один столбец из двенадцати</summary>
    [CssClass("1")]
    Template1 = 0,

    /// <summary>Два столбца из двенадцати</summary>
    [CssClass("2")]
    Template2 = 1,

    /// <summary>Три столбца из двенадцати</summary>
    [CssClass("3")]
    Template3 = 1 << 1,

    /// <summary>Четыре столбца из двенадцати</summary>
    [CssClass("4")]
    Template4 = 1 << 2,

    /// <summary>Пять столбцов из двенадцати</summary>
    [CssClass("5")]
    Template5 = 1 << 3,

    /// <summary>Шесть столбцов из двенадцати</summary>
    [CssClass("6")]
    Template6 = 1 << 4,

    /// <summary>Семь столбцов из двенадцати</summary>
    [CssClass("7")]
    Template7 = 1 << 5,

    /// <summary>Восемь столбцов из двенадцати</summary>
    [CssClass("8")]
    Template8 = 1 << 6,

    /// <summary>Девять столбцов из двенадцати</summary>
    [CssClass("9")]
    Template9 = 1 << 7,

    /// <summary>Десять столбцов из двенадцати</summary>
    [CssClass("10")]
    Template10 = 1 << 8,

    /// <summary>Одиннадцать столбцов из двенадцати</summary>
    [CssClass("11")]
    Template11 = 1 << 9,

    /// <summary>Двенадцать столбцов из двенадцати</summary>
    [CssClass("12")]
    Template12 = 1 << 10,

    /// <summary>Размер столбцов на основе естественной ширины их содержимого</summary>
    [CssClass("auto")]
    TemplateAuto = 1 << 11,
}
