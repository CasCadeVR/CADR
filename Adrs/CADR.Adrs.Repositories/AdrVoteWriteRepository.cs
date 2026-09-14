using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrVoteWriteRepository"/>
internal sealed class AdrVoteWriteRepository : BaseWriteRepository<AdrVote>,
    IAdrVoteWriteRepository,
    IAdrsRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrVoteWriteRepository"/>
    /// </summary>
    public AdrVoteWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
