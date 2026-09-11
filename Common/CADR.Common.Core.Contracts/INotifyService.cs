using System.Diagnostics.CodeAnalysis;
using CADR.Common.Core.Contracts.Models;

namespace CADR.Common.Core.Contracts;

/// <summary>
/// Сервис уведомлений
/// </summary>
public interface INotifyService
{
    /// <summary>
    /// Поставить уведомление в очередь на отправку
    /// </summary>
    /// <returns>Id уведомления</returns>
    Task<Guid> SendAsync([NotNull] NotifyModel model, CancellationToken cancellationToken);
}
