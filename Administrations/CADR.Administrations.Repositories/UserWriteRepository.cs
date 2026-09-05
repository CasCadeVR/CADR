using Cadr.Common.Repositories;
using CADR.Administrations.Entities;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Contracts;

namespace CADR.Administrations.Repositories;

/// <inheritdoc cref="IUserWriteRepository"/>
internal sealed class UserWriteRepository : BaseWriteRepository<User>,
    IUserWriteRepository,
    IAdministrationRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserWriteRepository"/>
    /// </summary>
    public UserWriteRepository(IDbWriterContext writerContext)
        : base(writerContext)
    {

    }
}
