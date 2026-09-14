namespace CADR.Adrs.Services.Contracts.Exceptions;

/// <summary>
/// Запрашиваемая сущность не найдена
/// </summary>
public class AdrEntityNotFoundException<TEntity> : AdrNotFoundException
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrEntityNotFoundException{TEntity}"/>
    /// </summary>
    public AdrEntityNotFoundException(Guid id)
        : base($"Сущность {typeof(TEntity)} c id = {id} не найдена.")
    {

    }
}
