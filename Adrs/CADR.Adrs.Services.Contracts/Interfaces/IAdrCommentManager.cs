using CADR.Adrs.Services.Contracts.Models.Comments;

namespace CADR.Adrs.Services.Contracts.Interfaces;

/// <summary>
/// Управление комментариями ADR
/// </summary>
public interface IAdrCommentManager
{
    /// <summary>
    /// Создаёт новый комментарий ADR
    /// </summary>
    Task<AdrCommentModel> CreateAsync(CreateAdrCommentModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список комментариев ADR
    /// </summary>
    Task<IEnumerable<AdrCommentModel>> GetByAdrIdAsync(Guid adrId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет существующий комментарий ADR
    /// </summary>
    Task<AdrCommentModel> UpdateAsync(UpdateAdrCommentModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет существующий комментарий ADR
    /// </summary>
    Task DeleteAsync(DeleteAdrCommentModel model, CancellationToken cancellationToken);
}
