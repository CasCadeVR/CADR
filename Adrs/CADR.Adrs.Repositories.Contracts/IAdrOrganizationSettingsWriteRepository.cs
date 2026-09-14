using CADR.Adrs.Entities;
using CADR.Common.Repositories.Contracts;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий записи <see cref="AdrOrganizationSettings"/>
/// </summary>
public interface IAdrOrganizationSettingsWriteRepository : IDbWriter<AdrOrganizationSettings> { }
