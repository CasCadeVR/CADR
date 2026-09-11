using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Определяет тип контейнера
/// </summary>
public enum Containers
{
    /// <summary>Контейнер по умолчанию</summary>
    [CssClass("container")]
    Default,

    /// <summary>Контейнер для <see langword="small"/></summary>
    [CssClass("container-sm")]
    Small,

    /// <summary>Контейнер для <see langword="medium"/></summary>
    [CssClass("container-md")]
    Medium,

    /// <summary>Контейнер для <see langword="large"/></summary>
    [CssClass("container-lg")]
    Large,

    /// <summary>Контейнер для <see langword="extra large"/></summary>
    [CssClass("container-xl")]
    ExtraLarge,

    /// <summary>Контейнер для <see langword="extra extra large"/></summary>
    [CssClass("container-xxl")]
    ExtraExtraLarge,

    /// <summary>Контейнер для <see langword="fluid"/></summary>
    [CssClass("container-fluid")]
    Fluid,
}
