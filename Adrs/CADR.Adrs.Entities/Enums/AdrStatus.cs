namespace CADR.Adrs.Entities.Enums;

/// <summary>
/// Статус ADR
/// </summary>
public enum AdrStatus
{
    /// <summary>
    /// ADR в черновике
    /// Может стать только <see cref="Proposed"/>
    /// </summary>
    Draft = 0,

    /// <summary>
    /// ADR голосуется, решается, стоит ли принимать, действует когда количество лайков = количеству дизлайков
    /// Может стать <see cref="Approved"/> - если admin подтвердит, или наберётся достаточное количество лайков
    /// Может стать <see cref="Rejected"/> - если admin отвергнет
    /// </summary>
    Proposed = 1,

    /// <summary>
    /// ADR утверждён и в силе
    /// Может стать <see cref="Deprecated"/> - если admin скажет
    /// Может стать <see cref="NeedsRevision"/> - если admin скажет, или количество дизлайков больше количества лайков, будет расмотрен на устарение
    /// </summary>
    Approved = 2,

    /// <summary>
    /// ADR отвержен
    /// Может стать <see cref="Approved"/> - если admin подтвердит, или наберётся достаточное количество лайков
    /// Может стать <see cref="Proposed"/> - если admin или создатель переделает
    /// </summary>
    Rejected = 3,

    /// <summary>
    /// ADR устарел
    /// </summary>
    Deprecated = 4,

    /// <summary>
    /// ADR со временем перестал быть актуальным, стоит пересмотреть его
    /// Может стать <see cref="Proposed"/> - если admin или создатель переделает
    /// </summary>
    NeedsRevision = 5,
}
