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
}
