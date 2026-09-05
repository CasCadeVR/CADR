namespace CADR.Common.Core.Contracts.Pagination;

/// <summary>
/// Страница пагинированного списка
/// </summary>
public abstract class CollectionPage<TItem>
{
    /// <summary>
    /// Объекты на странице
    /// </summary>
    public IReadOnlyCollection<TItem> Items { get; init; } = [];

    /// <summary>
    /// Всего элементов в исходной коллекции
    /// </summary>
    public int TotalCount { get; init; }
}
