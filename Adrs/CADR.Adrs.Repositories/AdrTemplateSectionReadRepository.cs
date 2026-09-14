using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrTemplateSectionReadRepository"/>
internal sealed class AdrTemplateSectionReadRepository : IAdrTemplateSectionReadRepository, IAdrsRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrTemplateSectionReadRepository"/>
    /// </summary>
    public AdrTemplateSectionReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<IReadOnlyCollection<AdrTemplateSection>> IAdrTemplateSectionReadRepository.GetByTemplateIdAsync(Guid templateId, CancellationToken cancellationToken)
        => reader.Read<AdrTemplateSection>()
            .Where(x => x.TemplateId == templateId)
            .NotDeletedAt()
            .OrderBy(x => x.Position)
            .ToReadOnlyCollectionAsync(cancellationToken);
}
