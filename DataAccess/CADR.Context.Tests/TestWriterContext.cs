using CADR.Common.Core.Contracts;
using CADR.Context.Contracts;
using Moq;

namespace CADR.Context.Tests;

/// <inheritdoc />
public class TestWriterContext : IDbWriterContext
{
    private readonly Mock<IDateTimeProvider> dateTimeProviderMock;
    private readonly Mock<IIdentityProvider> identityProviderMock;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TestWriterContext"/>
    /// </summary>
    public TestWriterContext(IWriter writer,
        IUnitOfWork unitOfWork)
    {
        Writer = writer;
        UnitOfWork = unitOfWork;

        dateTimeProviderMock = new Mock<IDateTimeProvider>();
        dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTimeOffset.UtcNow);

        identityProviderMock = new Mock<IIdentityProvider>();
        identityProviderMock.Setup(x => x.Name).Returns("test@identityProviderMock");
    }
    /// <inheritdoc />
    public IWriter Writer { get; }

    /// <inheritdoc />
    public IUnitOfWork UnitOfWork { get; }

    /// <inheritdoc />
    public IDateTimeProvider DateTimeProvider => dateTimeProviderMock.Object;

    /// <inheritdoc />
    public IIdentityProvider IdentityProvider => identityProviderMock.Object;
}
