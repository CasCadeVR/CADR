namespace CADR.Administrations.Services.Contracts.Models.Organizations;

/// <summary>
/// Модель организации
/// </summary>
public class OrganizationModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Флаг, указывающий, является ли пользователь, запрашиваемый организацию
    /// её администратором
    /// </summary>
    public bool UserIsAdmin { get; set; }
}
