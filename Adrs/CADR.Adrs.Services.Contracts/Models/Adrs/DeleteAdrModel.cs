namespace CADR.Adrs.Services.Contracts.Models.Adrs;

/// <summary>
/// Модель удаления ADR
/// </summary>
public class DeleteAdrModel
{
    /// <summary>
    /// Идентификатор пользователя, удаляющего ADR
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор ADR
    /// </summary>
    public Guid AdrId { get; set; }
}
