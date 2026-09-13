using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrCommentReadRepository"/>
internal sealed class AdrCommentReadRepository : IAdrCommentReadRepository, IAdrsRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrCommentReadRepository"/>
    /// </summary>
    public AdrCommentReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<AdrComment?> IAdrCommentReadRepository.GetActiveByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<AdrComment>()
            .ById(id)
            .NotDeletedAt()
            .SingleOrDefaultAsync(cancellationToken);

    Task<IReadOnlyCollection<AdrComment>> IAdrCommentReadRepository.GetByAdrIdAsync(Guid adrId, CancellationToken cancellationToken)
        => reader.Read<AdrComment>()
            .Where(x => x.AdrId == adrId)
            .NotDeletedAt()
            .OrderBy(x => x.CreatedAt)
            .ToReadOnlyCollectionAsync(cancellationToken);
}
