using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrLinkWriteRepository"/>
internal sealed class AdrLinkWriteRepository : BaseWriteRepository<AdrLink>,
    IAdrLinkWriteRepository,
    IAdrsRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrLinkWriteRepository"/>
    /// </summary>
    public AdrLinkWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
