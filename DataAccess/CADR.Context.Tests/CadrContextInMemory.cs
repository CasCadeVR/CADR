using CADR.Context;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CADR.Context.Tests;

/// <summary>
/// Класс <see cref="CadrContext"/> для тестов с базой в памяти. Один контекст на тест
/// </summary>
public class CadrContextInMemory : IAsyncDisposable
{
    /// <summary>
    /// Контекст <see cref="CadrContext"/>
    /// </summary>
    protected CadrContext Context { get; }

    /// <inheritdoc cref="IDbWriterContext"/>
    protected IDbWriterContext WriterContext => new TestWriterContext(Context, Context);

    /// <inheritdoc cref="IUnitOfWork"/>
    protected IUnitOfWork UnitOfWork => Context;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CadrContextInMemory"/>
    /// </summary>
    protected CadrContextInMemory()
    {
        var optionsBuilder = new DbContextOptionsBuilder<CadrContext>()
            .UseInMemoryDatabase($"CadrTests{Guid.NewGuid()}")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        Context = new CadrContext(optionsBuilder.Options);
    }

    /// <inheritdoc cref="IAsyncDisposable"/>
    public async ValueTask DisposeAsync()
    {
        await Context.Database.EnsureDeletedAsync();
        await Context.DisposeAsync();
    }
}
