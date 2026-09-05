using CADR.Common.Core.Contracts.Attributes;

namespace CADR.Common.Core.Contracts.Pagination;

/// <summary>
/// Запрос страницы коллекции
/// </summary>
/// <param name="PageNumber">Номер страницы</param>
/// <param name="PageSize">Размер страницы</param>
/// <param name="Filter">Фильтр</param>
public abstract record PagedCollectionRequest<TFilter>(int PageNumber, int PageSize, TFilter Filter) : FilteredCollectionRequest<TFilter>(Filter)
{
    /// <summary>
    /// Количество элементов с начала коллекции до начала запрошенной страницы
    /// </summary>
    [DocumentationIgnore]
    public int Offset => (PageNumber - 1) * PageSize;
}
