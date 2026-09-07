using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Button;

/// <summary>
/// Размер кнопки
/// </summary>
public enum ButtonSize
{
    /// <summary>Кнопка большого размера</summary>
    [CssClass("btn-lg")]
    Large,

    /// <summary>Кнопка меньшего размера</summary>
    [CssClass("btn-sm")]
    Small,
}
