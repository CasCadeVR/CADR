using System.ComponentModel.DataAnnotations;

namespace CADR.Administrations.Pages.Models.Organization;

/// <summary>
/// Модель формы создания и редактирования шаблона ADR
/// </summary>
public class AdrTemplateFormModel
{
    /// <summary>
    /// Название шаблона
    /// </summary>
    [Required(ErrorMessage = "Укажите название шаблона")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Секции шаблона
    /// </summary>
    public List<TemplateSectionEditModel> Sections { get; set; } = [];
}
