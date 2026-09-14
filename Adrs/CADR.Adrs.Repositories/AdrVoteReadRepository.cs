using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrVoteReadRepository"/>
internal sealed class AdrVoteReadRepository : IAdrVoteReadRepository, IAdrsRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrVoteReadRepository"/>
    /// </summary>
    public AdrVoteReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<AdrVote?> IAdrVoteReadRepository.GetByAdrAndUserIdAsync(Guid adrId, Guid userId, CancellationToken cancellationToken)
        => reader.Read<AdrVote>()
            .Where(x => x.AdrId == adrId)
            .Where(x => x.UserId == userId)
            .NotDeletedAt()
            .FirstOrDefaultAsync(cancellationToken);

    Task<IReadOnlyCollection<AdrVote>> IAdrVoteReadRepository.GetByAdrIdAsync(Guid adrId, CancellationToken cancellationToken)
        => reader.Read<AdrVote>()
            .Where(x => x.AdrId == adrId)
            .NotDeletedAt()
            .ToReadOnlyCollectionAsync(cancellationToken);
}
