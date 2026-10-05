using System.ComponentModel.DataAnnotations;

namespace CADR.Adrs.Pages.Models.Adrs;

/// <summary>
/// Модель формы создания и редактирования ADR
/// </summary>
public class AdrFormModel
{
    /// <summary>
    /// Название ADR
    /// </summary>
    [Required(ErrorMessage = "Укажите название ADR")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор используемого шаблона. Null если нет
    /// </summary>
    public Guid? TemplateId { get; set; }

    /// <summary>
    /// Секции ADR
    /// </summary>
    public List<SectionEditModel> Sections { get; set; } = [];
}
