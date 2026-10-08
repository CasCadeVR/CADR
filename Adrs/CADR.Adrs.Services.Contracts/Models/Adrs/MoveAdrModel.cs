namespace CADR.Adrs.Services.Contracts.Models.Adrs;

/// <summary>
/// Модель перемещения ADR в другую папку
/// </summary>
public class MoveAdrModel
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
