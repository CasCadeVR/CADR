using System.Diagnostics.CodeAnalysis;
using CADR.Common.Core.Contracts;
using CADR.Common.Core.Contracts.Models;

namespace CADR.Api.Stubs;

/// <summary>
/// Заглушка для <see cref="INotifyService"/>
/// </summary>
public class NotifyServiceStub : INotifyService
{
    Task<Guid> INotifyService.SendAsync([NotNull] NotifyModel model, CancellationToken cancellationToken)
        => Task.FromResult(Guid.Empty);
}
