using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Flex;

/// <summary>
/// Позволяет для ряда родственных элементов заставить их иметь ширину, равную их содержимому
/// (или равную ширину, если их содержимое не выходит за границы их рамок),
/// занимая при этом все доступное горизонтальное пространство
/// </summary>
public enum FlexFills
{
    /// <summary>Занимает все доступное горизонтальное пространство</summary>
    [CssClass("flex-fill")]
    Default,

    /// <summary>Занимает все доступное горизонтальное пространство для <see langword="small"/></summary>
    [CssClass("flex-sm-fill")]
    Small,

    /// <summary>Занимает все доступное горизонтальное пространство для <see langword="medium"/></summary>
    [CssClass("flex-md-fill")]
    Medium,

    /// <summary>Занимает все доступное горизонтальное пространство для <see langword="large"/></summary>
    [CssClass("flex-lg-fill")]
    Large,

    /// <summary>Занимает все доступное горизонтальное пространство для <see langword="extra large"/></summary>
    [CssClass("flex-xl-fill")]
    ExtraLarge,

    /// <summary>Занимает все доступное горизонтальное пространство для <see langword="extra extra large"/></summary>
    [CssClass("flex-xxl-fill")]
    ExtraExtraLarge,
}
