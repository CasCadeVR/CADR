using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Расположение выпадающего списка
/// </summary>
public enum DropDownDirections
{
    /// <summary>Список вверху</summary>
    [CssClass("dropup")]
    Dropup,

    /// <summary>Список справа</summary>
    [CssClass("dropend")]
    Dropright,

    /// <summary>Список слева</summary>
    [CssClass("dropstart")]
    Dropleft,
}
