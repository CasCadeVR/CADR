namespace CADR.Adrs.Entities.Enums;

/// <summary>
/// Вид связи одной ADR с другими ADR 
/// </summary>
public enum AdrLinkType
{
    /// <summary>
    /// ADR как-то связана с другой ADR (понятно по контексту содержимого раздела)
    /// </summary>
    RelatedTo = 0,

    /// <summary>
    /// ADR подавляет старую ADR
    /// (зеркало <see cref="DeprecatedBy"/>)
    /// </summary>
    Supersedes = 1,

    /// <summary>
    /// ADR подавлено новым ADR
    /// (зеркало <see cref="Supersedes"/>)
    /// </summary>
    DeprecatedBy = 2,
}
