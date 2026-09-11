namespace CADR.Context.Entities.Contracts.Interfaces;

/// <summary>
/// Аудит создания сущностей
/// </summary>
public interface IEntityAuditCreated
{
    /// <summary>
    /// Дата создания
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Кто создал
    /// </summary>
    string CreatedBy { get; set; }
}
