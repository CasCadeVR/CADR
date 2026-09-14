using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrFolderReadRepository"/>
internal sealed class AdrFolderReadRepository : IAdrFolderReadRepository, IAdrsRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrFolderReadRepository"/>
    /// </summary>
    public AdrFolderReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<AdrFolder?> IAdrFolderReadRepository.GetActiveByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<AdrFolder>()
            .ById(id)
            .NotDeletedAt()
            .SingleOrDefaultAsync(cancellationToken);

    Task<IReadOnlyCollection<AdrFolder>> IAdrFolderReadRepository.GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken)
        => reader.Read<AdrFolder>()
            .Where(x => x.OrganizationId == organizationId)
            .NotDeletedAt()
            .OrderBy(x => x.Name)
            .ToReadOnlyCollectionAsync(cancellationToken);

    Task<bool> IAdrFolderReadRepository.IsActiveNameExistsAsync(Guid organizationId, Guid? parentFolderId, string name, CancellationToken cancellationToken)
         => reader.Read<AdrFolder>()
            .Where(x => x.OrganizationId == organizationId)
            .Where(x => x.ParentAdrFolderId == parentFolderId)
            .Where(x => x.Name.ToLower() == name.ToLower())
            .NotDeletedAt()
            .AnyAsync(cancellationToken);
}
