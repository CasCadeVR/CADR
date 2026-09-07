namespace CADR.Portal.Components.Models;

/// <summary>
/// DTO-модель для карточки задачи
/// </summary>
public sealed record IssueCardItem
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Номер задачи
    /// </summary>
    public string Number { get; init; } = string.Empty;

    /// <summary>
    /// Тема
    /// </summary>
    public string Title { get; init; } = string.Empty;
}
