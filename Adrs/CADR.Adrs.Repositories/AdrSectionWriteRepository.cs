using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrSectionWriteRepository"/>
internal sealed class AdrSectionWriteRepository : BaseWriteRepository<AdrSection>,
    IAdrSectionWriteRepository,
    IAdrsRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrSectionWriteRepository"/>
    /// </summary>
    public AdrSectionWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
