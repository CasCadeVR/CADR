using CADR.Adrs.Entities;
using CADR.Common.Repositories.Contracts;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий записи <see cref="AdrTemplateSection"/>
/// </summary>
public interface IAdrTemplateSectionWriteRepository : IDbWriter<AdrTemplateSection> { }
