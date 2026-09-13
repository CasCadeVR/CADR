using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrTemplateSectionWriteRepository"/>
internal sealed class AdrTemplateSectionWriteRepository : BaseWriteRepository<AdrTemplateSection>,
    IAdrTemplateSectionWriteRepository,
    IAdrsRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrTemplateSectionWriteRepository"/>
    /// </summary>
    public AdrTemplateSectionWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
