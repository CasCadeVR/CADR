using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Common.Repositories;
using CADR.Context.Contracts;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrOrganizationSettingsWriteRepository"/>
internal sealed class AdrOrganizationSettingsWriteRepository : BaseWriteRepository<AdrOrganizationSettings>,
    IAdrOrganizationSettingsWriteRepository,
    IAdrsRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrOrganizationSettingsWriteRepository"/>
    /// </summary>
    public AdrOrganizationSettingsWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
