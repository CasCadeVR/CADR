using CADR.Context.Entities.Contracts.Interfaces;

namespace CADR.Context.Entities.Contracts.Models;

/// <summary>
/// Базовый класс сущностей с аудитом
/// </summary>
public abstract class BaseAuditEntity : IEntity,
    IEntityWithId,
    IEntityAuditCreated,
    IEntityAuditUpdate,
    IEntityAuditDeletedAt
{
    /// <inheritdoc cref="IEntityWithId"/>
    public Guid Id { get; set; }

    /// <inheritdoc cref="IEntityAuditCreated.CreatedAt"/>
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc cref="IEntityAuditCreated.CreatedBy"/>
    public string CreatedBy { get; set; } = string.Empty;

    /// <inheritdoc cref="IEntityAuditUpdate.UpdatedAt"/>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <inheritdoc cref="IEntityAuditUpdate.UpdatedBy"/>
    public string UpdatedBy { get; set; } = string.Empty;

    /// <inheritdoc cref="IEntityAuditDeletedAt.DeletedAt"/>
    public DateTimeOffset? DeletedAt { get; set; }
}
