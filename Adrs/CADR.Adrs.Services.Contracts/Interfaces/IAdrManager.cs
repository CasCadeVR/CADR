using CADR.Adrs.Services.Contracts.Models.Adrs;

namespace CADR.Adrs.Services.Contracts.Interfaces;

/// <summary>
/// Управление ADR
/// </summary>
public interface IAdrManager
{
    /// <summary>
    /// Создаёт новый ADR
    /// </summary>
    Task<AdrModel> CreateAdrAsync(CreateAdrModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает активный ADR по паре идентификатор ADR - идентификатор пользователя
    /// </summary>
    Task<AdrModel> GetByIdAsync(Guid adrId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех ADR организации
    /// </summary>
    Task<IEnumerable<AdrModel>> GetByOrganizationIdAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список ADR папки организации
    /// </summary>
    Task<IEnumerable<AdrModel>> GetByFolderIdAsync(Guid organizationId, Guid? folderId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список ADR автора в организации
    /// </summary>
    Task<IEnumerable<AdrModel>> GetByAuthorIdAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет существующий ADR вместе с разделами
    /// </summary>
    Task<AdrModel> UpdateAdrAsync(UpdateAdrModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет существующий ADR
    /// </summary>
    Task DeleteAdrAsync(DeleteAdrModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Меняет статус ADR
    /// </summary>
    Task ChangeStatusAsync(ChangeAdrStatusModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Голосует за ADR или меняет существующий голос
    /// </summary>
    Task VoteAsync(VoteAdrModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Снимает голос пользователя с ADR
    /// </summary>
    Task WithdrawVoteAsync(WithdrawVoteAdrModel model, CancellationToken cancellationToken);
}
