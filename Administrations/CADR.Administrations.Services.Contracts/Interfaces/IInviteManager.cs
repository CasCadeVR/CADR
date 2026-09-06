using CADR.Administrations.Services.Contracts.Models.Invites;

namespace CADR.Administrations.Services.Contracts.Interfaces;

/// <summary>
/// Управление приглашениями
/// </summary>
public interface IInviteManager
{
    /// <summary>
    /// Создание приглашения
    /// </summary>
    Task CreateInviteAsync(InviteModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список приглашений организации
    /// </summary>
    Task<IReadOnlyCollection<InviteOrganizationModel>> GetInvitesByOrganizationIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет созданное приглашение к организации
    /// </summary>
    Task DeleteInviteAsync(DeleteOrganizationInviteModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает приглашения к организациям, адресованные пользователю
    /// </summary>
    Task<IEnumerable<InviteForUserModel>> GetInvitesByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Принимает приглашение организации, адресованное пользователю
    /// </summary>
    Task AcceptInviteAsync(Guid inviteId, CancellationToken cancellationToken);

    /// <summary>
    /// Отклоняет приглашение организации, адресованное пользователю
    /// </summary>
    Task RejectInviteAsync(Guid inviteId, Guid userId, CancellationToken cancellationToken);
}
