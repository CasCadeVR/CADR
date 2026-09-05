using Cadr.Common.Repositories.Contracts;
using CADR.Administrations.Entities;

namespace CADR.Administrations.Repositories.Contracts;

/// <summary>
/// Репозиторий записи <see cref="Organization"/>
/// </summary>
public interface IOrganizationWriteRepository : IDbWriter<Organization> { }
