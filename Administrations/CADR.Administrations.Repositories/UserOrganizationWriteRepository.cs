using Cadr.Common.Repositories;
using CADR.Administrations.Entities;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Contracts;

namespace CADR.Administrations.Repositories;

/// <inheritdoc cref="IUserOrganizationWriteRepository"/>
internal sealed class UserOrganizationWriteRepository : BaseWriteRepository<UserOrganization>,
    IUserOrganizationWriteRepository,
    IAdministrationRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserOrganizationWriteRepository"/>
    /// </summary>
    public UserOrganizationWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
