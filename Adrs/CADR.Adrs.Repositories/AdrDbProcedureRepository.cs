using CADR.Administrations.Entities.Enums;
using CADR.Adrs.Entities.Enums;
using CADR.Adrs.Repositories.Contracts;
using CADR.Adrs.Repositories.Contracts.Models.Adrs;
using CADR.Context.Contracts;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrDbProcedureRepository"/>
internal sealed class AdrDbProcedureRepository : IAdrDbProcedureRepository, IAdrsRepositoryAnchor
{
    private readonly IReader reader;
    private readonly IUnitOfWork unitOfWork;

    public AdrDbProcedureRepository(IReader reader, IUnitOfWork unitOfWork)
    {
        this.reader = reader;
        this.unitOfWork = unitOfWork;
    }

    Task<CreateAdrResult> IAdrDbProcedureRepository.CreateAdrAsync(
        Guid organizationId, Guid authorId, Guid? folderId, string title, CancellationToken ct)
        => reader.SqlQueryAsync<CreateAdrResult>($"""
            SELECT * FROM create_adr({organizationId}, {authorId}, {folderId}, {title})
            """, ct)
            .ContinueWith(t => t.Result.Single(), ct);

    Task IAdrDbProcedureRepository.VoteAsync(Guid adrId, Guid userId, AdrVoteType value, CancellationToken ct)
        => unitOfWork.ExecuteSqlAsync(
            $"SELECT vote_adr({adrId}, {userId}, {(int)value})", ct);

    Task IAdrDbProcedureRepository.WithdrawVoteAsync(Guid adrId, Guid userId, CancellationToken ct)
        => unitOfWork.ExecuteSqlAsync(
            $"SELECT withdraw_vote_adr({adrId}, {userId})", ct);

    Task IAdrDbProcedureRepository.ChangeStatusAsync(Guid adrId, Role actorRole, AdrStatus newStatus, CancellationToken ct)
        => unitOfWork.ExecuteSqlAsync(
            $"SELECT change_adr_status({adrId}, {(int)actorRole}, {(int)newStatus})", ct);
}
