using CADR.Adrs.Entities;
using CADR.Common.Repositories.Contracts;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий записи <see cref="AdrTemplate"/>
/// </summary>
public interface IAdrTemplateWriteRepository : IDbWriter<AdrTemplate> { }
