using CADR.Adrs.Services.Contracts.Models.Templates;

namespace CADR.Adrs.Services.Contracts.Interfaces;

/// <summary>
/// Управление шаблонами ADR
/// </summary>
public interface IAdrTemplateManager
{
    /// <summary>
    /// Создаёт новый шаблон ADR организации
    /// </summary>
    Task<AdrTemplateModel> CreateAsync(CreateAdrTemplateModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает шаблон ADR по идентификатору вместе с разделами
    /// </summary>
    Task<AdrTemplateModel> GetByIdAsync(Guid templateId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список шаблонов, доступных организации (глобальные и собственные)
    /// </summary>
    Task<IEnumerable<AdrTemplateModel>> GetAvailableForOrganizationAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет существующий шаблон ADR организации
    /// </summary>
    Task<AdrTemplateModel> UpdateAsync(UpdateAdrTemplateModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет существующий шаблон ADR организации
    /// </summary>
    Task DeleteAsync(DeleteAdrTemplateModel model, CancellationToken cancellationToken);
}
