namespace CADR.Common.Core.Contracts.Models;

/// <summary>
/// Модель уведомления
/// </summary>
public class NotifyModel
{
    /// <summary>
    /// Основные получатели письменного уведомления
    /// </summary>
    public IReadOnlyCollection<string>? To { get; set; }

    /// <summary>
    /// Основные получатели уведомления
    /// </summary>
    public IReadOnlyCollection<Guid>? ToUsers { get; set; }

    /// <summary>
    /// Тема уведомления
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Содержимое уведомления
    /// </summary>
    public string Body { get; set; } = string.Empty;
}
