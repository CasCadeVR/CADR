using CADR.Portal.Contracts.Exceptions;

namespace CADR.Portal.Components;

/// <summary>
/// Компонент, реализующий IDisposable
/// </summary>
public class DisposableBootstrapComponentBase : BootstrapComponentBase, IDisposable
{
    /// <summary>
    /// Вызывался ли Dispose у компонента
    /// </summary>
    protected bool Disposed { get; private set; }

    /// <inheritdoc cref="CancellationTokenSource"/>
    protected CancellationTokenSource TokenSource { get; } = new();

    /// <summary>
    /// Бросить исключение, если компонент был утилизирован
    /// </summary>
    /// <exception cref="PortalObjectDisposedException"> У компонента уже вызывался Dispose </exception>
    protected void ThrowIfDisposed()
    {
        if (Disposed)
        {
            throw new PortalObjectDisposedException(this);
        }
    }

    /// <inheritdoc />
    public virtual void Dispose()
    {
        if (!Disposed)
        {
            TokenSource.Cancel();
            TokenSource.Dispose();
            Disposed = true;
        }
    }
}
