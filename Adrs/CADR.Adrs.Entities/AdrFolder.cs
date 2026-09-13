using CADR.Administrations.Entities;
using CADR.Context.Entities.Contracts.Models;

namespace CADR.Adrs.Entities;

/// <summary>
/// Папка, хранящая ADR в организации
/// </summary>
public class AdrFolder : BaseAuditEntity
{
    /// <summary>
    /// Имя папки
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Навигационное свойство организации
    /// </summary>
    public Organization? Organization { get; set; }

    /// <summary>
    /// Идентификатор родителя (папки)
    /// </summary>
    public Guid? ParentAdrFolderId { get; set; }

    /// <summary>
    /// Навигационное свойство родителя (папки)
    /// </summary>
    public AdrFolder? ParentAdrFolder { get; set; }

    /// <summary>
    /// Дочерние папки ADR
    /// </summary>
    public ICollection<AdrFolder> ChildFolder { get; set; } = [];

    /// <summary>
    /// Дочерние ADR
    /// </summary>
    public ICollection<Adr> Adrs { get; set; } = [];
}
