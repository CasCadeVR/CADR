using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Отображение компонентов
/// </summary>
public enum Displays
{
    /// <summary>None</summary>
    [CssClass("none")]
    None,

    /// <summary>Inline</summary>
    [CssClass("inline")]
    Inline,

    /// <summary>Inline block</summary>
    [CssClass("inline-block")]
    InlineBlock,

    /// <summary>Block</summary>
    [CssClass("block")]
    Block,

    /// <summary>Grid</summary>
    [CssClass("grid")]
    Grid,

    /// <summary>Table</summary>
    [CssClass("table")]
    Table,

    /// <summary>Table cell</summary>
    [CssClass("table-cell")]
    TableCell,

    /// <summary>Table row</summary>
    [CssClass("table-row")]
    TableRow,

    /// <summary>Flex</summary>
    [CssClass("flex")]
    Flex,

    /// <summary>Inline flex</summary>
    [CssClass("inline-flex")]
    InlineFlex,
}
