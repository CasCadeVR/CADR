using Cadr.Common.Repositories;
using CADR.Administrations.Entities;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CADR.Administrations.Repositories;

/// <inheritdoc cref="IRefreshTokenReadRepository"/>
internal sealed class RefreshTokenReadRepository : IRefreshTokenReadRepository, IAdministrationRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RefreshTokenReadRepository"/>
    /// </summary>
    public RefreshTokenReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<RefreshToken?> IRefreshTokenReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<RefreshToken>()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

    Task<RefreshToken?> IRefreshTokenReadRepository.GetActualByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<RefreshToken>()
            .ById(id)
            .NotDeletedAt()
            .FirstOrDefaultAsync(cancellationToken);

    Task<RefreshToken?> IRefreshTokenReadRepository.GetActualByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        => reader.Read<RefreshToken>()
            .NotDeletedAt()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);
}
