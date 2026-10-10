using CADR.Administrations.Entities.Enums;
using CADR.Adrs.Entities;
using CADR.Adrs.Entities.Enums;
using CADR.Adrs.Repositories.Contracts.Models.Adrs;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// Репозиторий на выполнения процедур для <see cref="Adr"/>
/// </summary>
public interface IAdrDbProcedureRepository
{
    /// <summary>
    /// Создаёт <see cref="Adr"/>
    /// </summary>
    Task<CreateAdrResult> CreateAdrAsync(
            Guid organizationId, Guid authorId, Guid? folderId, string title, CancellationToken cancellationToken);

    /// <summary>
    /// Проголосовать за <see cref="Adr"/>
    /// </summary>
    Task VoteAsync(Guid adrId, Guid userId, AdrVoteType value, CancellationToken cancellationToken);

    /// <summary>
    /// Снять голос у <see cref="Adr"/>
    /// </summary>
    Task WithdrawVoteAsync(Guid adrId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Поменять статус <see cref="Adr"/>
    /// </summary>
    Task ChangeStatusAsync(Guid adrId, Role actorRole, AdrStatus newStatus, CancellationToken cancellationToken);
}
