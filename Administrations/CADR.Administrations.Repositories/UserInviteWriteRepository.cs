using CADR.Common.Repositories;
using CADR.Administrations.Entities;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Contracts;

namespace CADR.Administrations.Repositories;

/// <inheritdoc cref="IUserInviteWriteRepository"/>
internal class UserInviteWriteRepository : BaseWriteRepository<UserInvite>,
    IUserInviteWriteRepository,
    IAdministrationRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserInviteWriteRepository"/>
    /// </summary>
    public UserInviteWriteRepository(IDbWriterContext writerContext)
        : base(writerContext)
    {
    }
}
