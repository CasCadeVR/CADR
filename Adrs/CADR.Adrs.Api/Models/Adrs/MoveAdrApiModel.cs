namespace CADR.Adrs.Api.Models.Adrs;

/// <summary>
/// API Модель перемещения ADR в другую папку
/// </summary>
public class MoveAdrApiModel
{
    /// <summary>
    /// Идентификатор пользователя, обновляющий ADR
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор родителя (папки)
    /// </summary>
    public Guid? ParentAdrFolderId { get; set; }
}
