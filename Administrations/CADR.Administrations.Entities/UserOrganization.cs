using CADR.Administrations.Entities.Enums;
using CADR.Context.Entities.Contracts.Models;

namespace CADR.Administrations.Entities;

/// <summary>
/// Организации пользователя
/// </summary>
public class UserOrganization : BaseAuditEntity
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Навигационное свойство пользователя
    /// </summary>
    public virtual User? User { get; set; }

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid? OrganizationId { get; set; }

    /// <summary>
    /// Навигационное свойство организации
    /// </summary>
    public virtual Organization? Organization { get; set; }

    /// <summary>
    /// Роль пользователя
    /// </summary>
    public Role Role { get; set; }
}
