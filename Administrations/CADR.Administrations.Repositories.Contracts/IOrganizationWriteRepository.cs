using CADR.Administrations.Entities;
using CADR.Common.Repositories.Contracts;

namespace CADR.Administrations.Repositories.Contracts;

/// <summary>
/// Репозиторий записи <see cref="Organization"/>
/// </summary>
public interface IOrganizationWriteRepository : IDbWriter<Organization> { }
