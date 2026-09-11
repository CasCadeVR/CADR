namespace CADR.Administrations.Services.Contracts.Exceptions;

/// <summary>
/// Запрашиваемая сущность не найдена
/// </summary>
public class AdministrationEntityNotFoundException<TEntity> : AdministrationNotFoundException
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationEntityNotFoundException{TEntity}"/>
    /// </summary>
    public AdministrationEntityNotFoundException(Guid id)
        : base($"Сущность {typeof(TEntity)} c id = {id} не найдена.")
    {

    }
}
