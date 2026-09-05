namespace CADR.Common.Core.Contracts.Pagination;

/// <summary>
/// Запрос отфильтрованной коллекции
/// </summary>
/// <param name="Filter">Фильтр</param>
public abstract record FilteredCollectionRequest<TFilter>(TFilter Filter);
