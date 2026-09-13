using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrWriteRepository"/>
internal sealed class AdrWriteRepository : BaseWriteRepository<Adr>,
    IAdrWriteRepository,
    IAdrsRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrWriteRepository"/>
    /// </summary>
    public AdrWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
