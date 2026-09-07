using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Характеристики поведения изображения
/// </summary>
public enum ImageAppearances
{
    /// <summary>Растягивать изображение</summary>
    [CssClass("img-fluid")]
    Fluid = 0,

    /// <summary>Добавляет изображению скруглённую рамку в 1 пиксель</summary>
    [CssClass("img-thumbnail")]
    Thumbnail = 1,
}
