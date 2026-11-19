using CADR.Administrations.Entities.Enums;
using CADR.Context.Entities.Contracts.Interfaces;

namespace CADR.Administrations.Entities.Views;

/// <summary>
/// Вид подтянутой связи  с использованием VIEW
/// </summary>
public class OrganizationMemberView : IEntity, IEntityAuditDeletedAt
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid? OrganizationId { get; set; }

    /// <summary>
    /// Роль пользователя
    /// </summary>
    public Role Role { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Логин пользователя
    /// </summary>
    public string UserLogin { get; set; } = null!;

    /// <summary>
    /// Email пользователя
    /// </summary>
    public string UserEmail { get; set; } = null!;

    /// <summary>
    /// Заблокирован ли пользователь
    /// </summary>
    public bool UserBlocked { get; set; }

    /// <summary>
    /// Время действия блокировки
    /// </summary>
    public DateTimeOffset? UserBlockedAt { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Дата удаления (если soft deleted)
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    DateTimeOffset? IEntityAuditDeletedAt.DeletedAt { get => DeletedAt; set => throw new NotImplementedException(); }
}
