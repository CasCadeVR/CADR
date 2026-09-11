namespace CADR.Context.Entities.Contracts.Interfaces;

/// <summary>
/// Аудит редактирования сущностей
/// </summary>
public interface IEntityAuditUpdate
{
    /// <summary>
    /// Дата изменения
    /// </summary>
    DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Кто изменил
    /// </summary>
    string UpdatedBy { get; set; }
}
