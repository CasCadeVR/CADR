using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrReadRepository"/>
internal sealed class AdrReadRepository : IAdrReadRepository, IAdrsRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrReadRepository"/>
    /// </summary>
    public AdrReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<Adr?> IAdrReadRepository.GetActiveByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<Adr>()
            .ById(id)
            .NotDeletedAt()
            .SingleOrDefaultAsync(cancellationToken);

    Task<IReadOnlyCollection<Adr>> IAdrReadRepository.GetByAuthorIdAsync(Guid organizationId, Guid authorId, CancellationToken cancellationToken)
        => reader.Read<Adr>()
            .Where(x => x.OrganizationId == organizationId)
            .Where(x => x.AuthorId == authorId)
            .NotDeletedAt()
            .ToReadOnlyCollectionAsync(cancellationToken);

    Task<IReadOnlyCollection<Adr>> IAdrReadRepository.GetByFolderIdAsync(Guid organizationId, Guid? folderId, CancellationToken cancellationToken)
        => reader.Read<Adr>()
            .Where(x => x.OrganizationId == organizationId)
            .Where(x => x.ParentAdrFolderId == folderId)
            .NotDeletedAt()
            .ToReadOnlyCollectionAsync(cancellationToken);

    Task<IReadOnlyCollection<Adr>> IAdrReadRepository.GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken)
        => reader.Read<Adr>()
            .Where(x => x.OrganizationId == organizationId)
            .NotDeletedAt()
            .ToReadOnlyCollectionAsync(cancellationToken);

    Task<int> IAdrReadRepository.GetCountByFolderIdsAsync(IReadOnlyCollection<Guid> folderIds, CancellationToken cancellationToken)
        => reader.Read<Adr>()
            .Where(x => x.ParentAdrFolderId != null && folderIds.Contains(x.ParentAdrFolderId.Value))
            .NotDeletedAt()
            .CountAsync(cancellationToken);

    Task<int> IAdrReadRepository.GetMaxNumberAsync(Guid organizationId, CancellationToken cancellationToken)
        => reader.Read<Adr>()
            .Where(x => x.OrganizationId == organizationId)
            .MaxAsync(x => (int?)x.Number, cancellationToken)
            .ContinueWith(t => t.Result ?? 0, cancellationToken);

    Task<bool> IAdrReadRepository.IsActiveNumberExistsAsync(Guid organizationId, int number, CancellationToken cancellationToken)
        => reader.Read<Adr>()
            .Where(x => x.OrganizationId == organizationId)
            .Where(x => x.Number == number)
            .NotDeletedAt()
            .AnyAsync(cancellationToken);
}
