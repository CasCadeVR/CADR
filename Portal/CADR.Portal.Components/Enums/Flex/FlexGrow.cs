using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums.Flex;

/// <summary>
/// Способность гибкого элемента увеличиваться, чтобы заполнить доступное пространство
/// </summary>
public enum FlexGrow
{
    /// <summary>Элементу предоставляется необходимое пространство</summary>
    [CssClass("flex-grow-0")]
    Grow0,

    /// <summary>Элементу предоставляется всё доступное пространство</summary>
    [CssClass("flex-grow-1")]
    Grow1,

    /// <summary>Элементу предоставляется необходимое пространство для <see langword="small"/></summary>
    [CssClass("flex-sm-grow-0")]
    GrowSmall0,

    /// <summary>Элементу предоставляется всё доступное пространство для <see langword="small"/></summary>
    [CssClass("flex-sm-grow-1")]
    GrowSmall1,

    /// <summary>Элементу предоставляется необходимое пространство для <see langword="medium"/></summary>
    [CssClass("flex-md-grow-0")]
    GrowMedium0,

    /// <summary>Элементу предоставляется всё доступное пространство для <see langword="medium"/></summary>
    [CssClass("flex-md-grow-1")]
    GrowMedium1,

    /// <summary>Элементу предоставляется необходимое пространство для <see langword="large"/></summary>
    [CssClass("flex-lg-grow-0")]
    GrowLarge0,

    /// <summary>Элементу предоставляется всё доступное пространство для <see langword="large"/></summary>
    [CssClass("flex-lg-grow-1")]
    GrowLarge1,

    /// <summary>Элементу предоставляется необходимое пространство для <see langword="extra large"/></summary>
    [CssClass("flex-xl-grow-0")]
    GrowExtraLarge0,

    /// <summary>Элементу предоставляется всё доступное пространство для <see langword="extra large"/></summary>
    [CssClass("flex-xl-grow-1")]
    GrowExtraLarge1,

    /// <summary>Элементу предоставляется необходимое пространство для <see langword="extra extra large"/></summary>
    [CssClass("flex-xxl-grow-0")]
    GrowExtraExtraLarge0,

    /// <summary>Элементу предоставляется всё доступное пространство для <see langword="extra extra large"/></summary>
    [CssClass("flex-xxl-grow-1")]
    GrowExtraExtraLarge1,
}
