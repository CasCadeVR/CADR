using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrTemplateWriteRepository"/>
internal sealed class AdrTemplateWriteRepository : BaseWriteRepository<AdrTemplate>,
    IAdrTemplateWriteRepository,
    IAdrsRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrTemplateWriteRepository"/>
    /// </summary>
    public AdrTemplateWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
