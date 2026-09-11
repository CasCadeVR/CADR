using CADR.Administrations.Repositories;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Contracts;
using Moq;

namespace CADR.Administrations.Services.Tests.UnitOfWork;

/// <summary>
/// Содержит все UoF, требуемые для тестов
/// </summary>
public class TestUnitOfWork
{
    /// <summary>
    /// UoF из Administration
    /// </summary>
    public IAdministrationUnitOfWork AdministrationUnitOfWork { get; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TestUnitOfWork"/>
    /// </summary>
    public TestUnitOfWork(IDbWriterContext writerContext, IReader reader, IUnitOfWork unitOfWork)
    {
        var administrationUnitOfWorkMock = new Mock<IAdministrationUnitOfWork>();
        administrationUnitOfWorkMock.Setup(x => x.OrganizationReadRepository).Returns(new OrganizationReadRepository(reader));
        administrationUnitOfWorkMock.Setup(x => x.OrganizationWriteRepository).Returns(new OrganizationWriteRepository(writerContext));
        administrationUnitOfWorkMock.Setup(x => x.RefreshTokenReadRepository).Returns(new RefreshTokenReadRepository(reader));
        administrationUnitOfWorkMock.Setup(x => x.RefreshTokenWriteRepository).Returns(new RefreshTokenWriteRepository(writerContext));
        administrationUnitOfWorkMock.Setup(x => x.UserOrganizationReadRepository).Returns(new UserOrganizationReadRepository(reader));
        administrationUnitOfWorkMock.Setup(x => x.UserOrganizationWriteRepository).Returns(new UserOrganizationWriteRepository(writerContext));
        administrationUnitOfWorkMock.Setup(x => x.UserReadRepository).Returns(new UserReadRepository(reader));
        administrationUnitOfWorkMock.Setup(x => x.UserWriteRepository).Returns(new UserWriteRepository(writerContext));
        administrationUnitOfWorkMock.Setup(x => x.UserReadRepository).Returns(new UserReadRepository(reader));
        administrationUnitOfWorkMock.Setup(x => x.UserInviteReadRepository).Returns(new UserInviteReadRepository(reader));
        administrationUnitOfWorkMock.Setup(x => x.UserInviteWriteRepository).Returns(new UserInviteWriteRepository(writerContext));
        administrationUnitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns((CancellationToken token) => unitOfWork.SaveChangesAsync(token));

        AdministrationUnitOfWork = administrationUnitOfWorkMock.Object;
    }
}
