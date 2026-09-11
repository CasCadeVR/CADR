namespace CADR.Context.Entities.Contracts.Interfaces;

/// <summary>
/// Сущность с идентификатором
/// </summary>
public interface IEntityWithId
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    Guid Id { get; set; }
}
