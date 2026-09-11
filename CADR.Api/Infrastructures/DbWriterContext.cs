using CADR.Common.Core.Contracts;
using CADR.Context.Contracts;

namespace CADR.Api.Infrastructures;

/// <inheritdoc cref="IDbWriterContext"/>
public class DbWriterContext : IDbWriterContext
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="DbWriterContext"/>
    /// </summary>
    public DbWriterContext(IWriter writer,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IIdentityProvider identityProvider)
    {
        Writer = writer;
        UnitOfWork = unitOfWork;
        DateTimeProvider = dateTimeProvider;
        IdentityProvider = identityProvider;
    }

    /// <inheritdoc cref="IDbWriterContext.Writer"/>
    public IWriter Writer { get; }

    /// <inheritdoc cref="IDbWriterContext.UnitOfWork"/>
    public IUnitOfWork UnitOfWork { get; }

    /// <inheritdoc cref="IDbWriterContext.DateTimeProvider"/>
    public IDateTimeProvider DateTimeProvider { get; }

    /// <inheritdoc cref="IDbWriterContext.IdentityProvider"/>
    public IIdentityProvider IdentityProvider { get; }
}
