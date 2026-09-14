using CADR.Adrs.Entities;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="AdrTemplateSection"/>
/// </summary>
public interface IAdrTemplateSectionReadRepository
{
    /// <summary>
    /// Получает список всех секций одного шаблона ADR
    /// </summary>
    Task<IReadOnlyCollection<AdrTemplateSection>> GetByTemplateIdAsync(Guid templateId, CancellationToken cancellationToken);
}
