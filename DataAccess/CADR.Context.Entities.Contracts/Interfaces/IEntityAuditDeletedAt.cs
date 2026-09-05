namespace CADR.Context.Entities.Contracts.Interfaces;

/// <summary>
/// Аудит удаления сущностей
/// </summary>
public interface IEntityAuditDeletedAt
{
    /// <summary>
    /// Дата удаления
    /// </summary>
    DateTimeOffset? DeletedAt { get; set; }
}
