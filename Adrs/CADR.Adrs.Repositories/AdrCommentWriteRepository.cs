using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrCommentWriteRepository"/>
internal sealed class AdrCommentWriteRepository : BaseWriteRepository<AdrComment>,
    IAdrCommentWriteRepository,
    IAdrsRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrCommentWriteRepository"/>
    /// </summary>
    public AdrCommentWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
