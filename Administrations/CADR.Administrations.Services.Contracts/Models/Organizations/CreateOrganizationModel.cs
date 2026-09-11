namespace CADR.Administrations.Services.Contracts.Models.Organizations;

/// <summary>
/// Модель создания организиции
/// </summary>
public class CreateOrganizationModel
{
    /// <summary>
    /// Идентификатор пользователя, создающего организацию
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
