using CADR.Adrs.Entities;
using CADR.Common.Repositories.Contracts;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий записи <see cref="AdrLink"/>
/// </summary>
public interface IAdrLinkWriteRepository : IDbWriter<AdrLink> { }
