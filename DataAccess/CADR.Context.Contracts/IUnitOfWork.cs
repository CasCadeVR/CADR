namespace CADR.Context.Contracts;

/// <summary>
/// Определяет интерфейс для unit of work
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Асинхронно сохраняет все изменения контекста
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Выполнить SQL комманду (процедуру)
    /// </summary>
    Task<int> ExecuteSqlAsync(FormattableString sql, CancellationToken cancellationToken = default);
}
