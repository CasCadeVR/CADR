using CADR.Adrs.Entities;
using CADR.Common.Repositories.Contracts;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий записи <see cref="AdrVote"/>
/// </summary>
public interface IAdrVoteWriteRepository : IDbWriter<AdrVote> { }
