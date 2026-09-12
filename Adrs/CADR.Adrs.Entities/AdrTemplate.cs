using CADR.Administrations.Entities;
using CADR.Context.Entities.Contracts.Models;

namespace CADR.Adrs.Entities;

/// <summary>
/// Шаблон ADR
/// </summary>
public class AdrTemplate : BaseAuditEntity
{
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
    /// Навигационное свойство организации
    /// </summary>
    public Organization? Organization { get; set; }

    /// <summary>
    /// Разделы шаблона
    /// </summary>
    public ICollection<AdrTemplateSection> Sections { get; set; } = [];
}
