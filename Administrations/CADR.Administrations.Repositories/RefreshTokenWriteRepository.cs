using CADR.Common.Repositories;
using CADR.Administrations.Entities;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Contracts;

namespace CADR.Administrations.Repositories;

/// <inheritdoc cref="IRefreshTokenWriteRepository"/>
internal sealed class RefreshTokenWriteRepository : BaseWriteRepository<RefreshToken>,
    IRefreshTokenWriteRepository,
    IAdministrationRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RefreshTokenWriteRepository"/>
    /// </summary>
    public RefreshTokenWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
