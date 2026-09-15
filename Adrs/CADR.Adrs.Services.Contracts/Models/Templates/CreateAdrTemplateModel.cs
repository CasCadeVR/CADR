namespace CADR.Adrs.Services.Contracts.Models.Templates;

/// <summary>
/// Модель создания шаблона ADR
/// </summary>
public class CreateAdrTemplateModel
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
    public IReadOnlyCollection<CreateAdrTemplateSectionModel> Sections { get; set; } = [];
}
