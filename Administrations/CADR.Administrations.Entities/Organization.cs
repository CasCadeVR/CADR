using CADR.Context.Entities.Contracts.Models;

namespace CADR.Administrations.Entities;

/// <summary>
/// Организация
/// </summary>
public class Organization : BaseAuditEntity
{
    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Значение <see cref="Name"/> в нижнем регистре
    /// </summary>
    public string NameLowerCase { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Пользователи организации
    /// </summary>
    public ICollection<UserOrganization> Users { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="Organization"/>
    /// </summary>
    public Organization()
    {
        Users = new List<UserOrganization>();
    }
}
