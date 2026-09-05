using System.Diagnostics.CodeAnalysis;
using CADR.Context.Entities.Contracts.Interfaces;

namespace CADR.Context.Contracts;

/// <summary>
/// Интерфейс создания и модификации записей в контексте CADR
/// </summary>
public interface IWriter
{
    /// <summary>
    /// Добавить новую запись
    /// </summary>
    void Add<TEntity>([NotNull] TEntity entity) where TEntity : class, IEntity;

    /// <summary>
    /// Изменить запись
    /// </summary>
    void Update<TEntity>([NotNull] TEntity entity) where TEntity : class, IEntity;

    /// <summary>
    /// Удалить запись
    /// </summary>
    void Delete<TEntity>([NotNull] TEntity entity) where TEntity : class, IEntity;
}
