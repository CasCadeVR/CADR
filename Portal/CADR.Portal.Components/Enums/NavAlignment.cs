using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Выравнивание элементов меню
/// </summary>
public enum NavAlignment
{
    /// <summary>Равномерное заполнение элементов</summary>
    [CssClass("nav-fill")]
    Fill,

    /// <summary>Элементы на всю длину контейнера</summary>
    [CssClass("nav-justified")]
    Justified,
}
