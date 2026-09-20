namespace CADR.Adrs.Api.Models.Adrs;

/// <summary>
/// API Модель удаления ADR
/// </summary>
public class DeleteAdrApiModel
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
