namespace CADR.Adrs.Api.Models.Templates;

/// <summary>
/// API Модель шаблона ADR
/// </summary>
public class AdrTemplateApiModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя шаблона 
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор организации
    /// null - если шаблон глобальный
    /// </summary>
    public Guid? OrganizationId { get; set; }

    /// <summary>
    /// Встроен ли он глобально 
    /// </summary>
    public bool IsBuiltIn { get; set; }

    /// <summary>
    /// Разделы шаблона
    /// </summary>
    public IReadOnlyCollection<AdrTemplateSectionApiModel> Sections { get; set; } = [];
}
