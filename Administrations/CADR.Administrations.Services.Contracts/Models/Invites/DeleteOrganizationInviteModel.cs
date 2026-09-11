namespace CADR.Administrations.Services.Contracts.Models.Invites;

/// <summary>
/// Модель удаления приглашения к организации
/// </summary>
public class DeleteOrganizationInviteModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Идентификатор приглашения
    /// </summary>
    public Guid InviteId { get; set; }
}
