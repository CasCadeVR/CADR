using CADR.Adrs.Entities;
using CADR.Common.Repositories.Contracts;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий записи <see cref="AdrSection"/>
/// </summary>
public interface IAdrSectionWriteRepository : IDbWriter<AdrSection> { }
