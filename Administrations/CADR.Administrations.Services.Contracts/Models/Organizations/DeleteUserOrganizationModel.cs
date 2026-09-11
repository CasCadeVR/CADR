namespace CADR.Administrations.Services.Contracts.Models.Organizations;

/// <summary>
/// Модель запроса на удаление пользователя из организации
/// </summary>
public class DeleteUserOrganizationModel
{
    /// <summary>
    /// Идентификатор пользователя, отправившего запрос
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Идентификатор пользователя на удаление
    /// </summary>
    public Guid UserToDeleteId { get; set; }
}
