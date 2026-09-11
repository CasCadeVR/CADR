using CADR.Context.Entities.Contracts.Interfaces;

namespace CADR.Context.Contracts;

/// <summary>
/// Интерфейс получение записей из контекста
/// </summary>
public interface IReader
{
    /// <summary>
    /// Предоставляет функциональные возможности для выполнения запросов
    /// </summary>
    IQueryable<TEntity> Read<TEntity>() where TEntity : class, IEntity;
}
