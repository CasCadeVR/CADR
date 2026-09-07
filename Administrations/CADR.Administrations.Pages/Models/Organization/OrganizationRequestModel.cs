using System.ComponentModel.DataAnnotations;

namespace CADR.Administrations.Pages.Models.Organization;

/// <summary>
/// Модель запроса организации
/// </summary>
public class OrganizationRequestModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    [Required(ErrorMessage = "Укажите наименование организации")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
