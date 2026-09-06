namespace CADR.Administrations.Services.Contracts.Models.Organizations;

/// <summary>
/// Модель обновления организации
/// </summary>
public class UpdateOrganizationModel : OrganizationModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
}
