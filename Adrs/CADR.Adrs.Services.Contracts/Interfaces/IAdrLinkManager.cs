using CADR.Adrs.Services.Contracts.Models.Links;

namespace CADR.Adrs.Services.Contracts.Interfaces;

/// <summary>
/// Управление связями ADR
/// </summary>
public interface IAdrLinkManager
{
    /// <summary>
    /// Создаёт новую связь между ADR
    /// </summary>
    Task<AdrLinkModel> CreateAsync(CreateAdrLinkModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех связей ADR (в обе стороны)
    /// </summary>
    Task<IEnumerable<AdrLinkModel>> GetByAdrIdAsync(Guid adrId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет существующую связь ADR
    /// </summary>
    Task DeleteAsync(DeleteAdrLinkModel model, CancellationToken cancellationToken);
}
