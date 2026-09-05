using Cadr.Common.Repositories;
using CADR.Administrations.Entities;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Contracts;

namespace CADR.Administrations.Repositories;

/// <inheritdoc cref="IOrganizationWriteRepository"/>
internal sealed class OrganizationWriteRepository : BaseWriteRepository<Organization>,
    IOrganizationWriteRepository,
    IAdministrationRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationWriteRepository"/>
    /// </summary>
    public OrganizationWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
