using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrFolderWriteRepository"/>
internal sealed class AdrFolderWriteRepository : BaseWriteRepository<AdrFolder>,
    IAdrFolderWriteRepository,
    IAdrsRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrFolderWriteRepository"/>
    /// </summary>
    public AdrFolderWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
