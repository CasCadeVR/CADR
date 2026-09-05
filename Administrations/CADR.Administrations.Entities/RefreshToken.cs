using CADR.Context.Entities.Contracts.Interfaces;

namespace CADR.Administrations.Entities;

/// <summary>
/// Токен обновления токена доступа
/// </summary>
public class RefreshToken : IEntity,
    IEntityWithId,
    IEntityAuditDeletedAt
{
    /// <inheritdoc cref="IEntityWithId"/>
    public Guid Id { get; set; }

    /// <summary>
    /// Дата и время срока действия
    /// </summary>
    public DateTimeOffset Expires { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Отметка безопасности
    /// </summary>
    public string SecurityStamp { get; set; } = string.Empty;

    /// <summary>
    /// Полезная информация токена доступа
    /// </summary>
    public string AccessPayload { get; set; } = string.Empty;

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc cref="IEntityAuditDeletedAt.DeletedAt"/>
    public DateTimeOffset? DeletedAt { get; set; }
}
