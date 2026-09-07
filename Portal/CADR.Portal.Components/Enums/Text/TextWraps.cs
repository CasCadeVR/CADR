using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Text;

/// <summary>
/// Перенос слов
/// </summary>
public enum TextWraps
{
    /// <summary>Переносить</summary>
    [CssClass("text-wrap")]
    Wrap,

    /// <summary>Не переносить</summary>
    [CssClass("text-nowrap")]
    Nowrap,
}
