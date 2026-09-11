namespace CADR.Administrations.Api.Models.Organizations;

/// <summary>
/// Api модель создания организиции
/// </summary>
public class CreateOrganizationApiModel
{
    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
