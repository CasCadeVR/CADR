using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Button;

/// <summary>
/// Тип закрытия кнопки
/// </summary>
public enum ButtonCloseType
{
    /// <summary>
    /// Не отображать кнопку закрытия
    /// </summary>
    [CssClass("")]
    None = 0,

    /// <summary>
    /// Отображать кнопку закрытия с белым крестиком
    /// </summary>
    [CssClass("btn-close btn-close-white")]
    White = 1,

    /// <summary>
    /// Отображать кнопку закрытия с черным крестиком
    /// </summary>
    [CssClass("btn-close btn-close-black")]
    Black = 2
}
