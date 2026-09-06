using System.Collections.ObjectModel;

namespace CADR.Common.Core.Extensions;

/// <summary>
/// Расширения для работы с перечислениями
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Возвращает readonly коллекцию.
    /// </summary>
    /// <param name="source">Последовательность элементов</param>
    /// <typeparam name="T">Тип элемента</typeparam>
    public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
    {
        var list = source as IList<T> ?? source.ToList();
        return new ReadOnlyCollection<T>(list);
    }
}
