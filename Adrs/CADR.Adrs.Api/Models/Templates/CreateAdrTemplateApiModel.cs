namespace CADR.Adrs.Api.Models.Templates;

/// <summary>
/// API Модель создания шаблона ADR
/// </summary>
public class CreateAdrTemplateApiModel
{
    /// <summary>
    /// Имя шаблона
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Идентификатор пользователя, создающий шаблон
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Разделы шаблона
    /// </summary>
    public IReadOnlyCollection<CreateAdrTemplateSectionApiModel> Sections { get; set; } = [];
}
