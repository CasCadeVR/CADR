using CADR.Administrations.Repositories;
using CADR.Administrations.Repositories.Contracts;
using CADR.Adrs.Repositories;
using CADR.Adrs.Repositories.Contracts;
using CADR.Context.Contracts;
using Moq;

namespace CADR.Adrs.Services.Tests.UnitOfWork;

/// <summary>
/// Содержит все UoF, требуемые для тестов
/// </summary>
public class TestUnitOfWork
{
    /// <summary>
    /// UoF из Adrs
    /// </summary>
    public IAdrsUnitOfWork AdrUnitOfWork { get; }

    /// <summary>
    /// Репозиторий на чтение <see cref="CADR.Administrations.Entities.UserOrganization"/>
    /// </summary>
    public IUserOrganizationReadRepository UserOrganizationReadRepository { get; }

    /// <summary>
    /// Репозиторий на чтение <see cref="CADR.Administrations.Entities.User"/>
    /// </summary>
    public IUserReadRepository UserReadRepository { get; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TestUnitOfWork"/>
    /// </summary>
    public TestUnitOfWork(IDbWriterContext writerContext, IReader reader, IUnitOfWork unitOfWork)
    {
        var adrUnitOfWorkMock = new Mock<IAdrsUnitOfWork>();
        adrUnitOfWorkMock.Setup(x => x.AdrCommentReadRepository).Returns(new AdrCommentReadRepository(reader));
        adrUnitOfWorkMock.Setup(x => x.AdrCommentWriteRepository).Returns(new AdrCommentWriteRepository(writerContext));
        adrUnitOfWorkMock.Setup(x => x.AdrFolderReadRepository).Returns(new AdrFolderReadRepository(reader));
        adrUnitOfWorkMock.Setup(x => x.AdrFolderWriteRepository).Returns(new AdrFolderWriteRepository(writerContext));
        adrUnitOfWorkMock.Setup(x => x.AdrLinkReadRepository).Returns(new AdrLinkReadRepository(reader));
        adrUnitOfWorkMock.Setup(x => x.AdrLinkWriteRepository).Returns(new AdrLinkWriteRepository(writerContext));
        adrUnitOfWorkMock.Setup(x => x.AdrOrganizationSettingsReadRepository).Returns(new AdrOrganizationSettingsReadRepository(reader));
        adrUnitOfWorkMock.Setup(x => x.AdrOrganizationSettingsWriteRepository).Returns(new AdrOrganizationSettingsWriteRepository(writerContext));
        adrUnitOfWorkMock.Setup(x => x.AdrReadRepository).Returns(new AdrReadRepository(reader));
        adrUnitOfWorkMock.Setup(x => x.AdrWriteRepository).Returns(new AdrWriteRepository(writerContext));
        adrUnitOfWorkMock.Setup(x => x.AdrSectionReadRepository).Returns(new AdrSectionReadRepository(reader));
        adrUnitOfWorkMock.Setup(x => x.AdrSectionWriteRepository).Returns(new AdrSectionWriteRepository(writerContext));
        adrUnitOfWorkMock.Setup(x => x.AdrTemplateReadRepository).Returns(new AdrTemplateReadRepository(reader));
        adrUnitOfWorkMock.Setup(x => x.AdrTemplateWriteRepository).Returns(new AdrTemplateWriteRepository(writerContext));
        adrUnitOfWorkMock.Setup(x => x.AdrTemplateSectionReadRepository).Returns(new AdrTemplateSectionReadRepository(reader));
        adrUnitOfWorkMock.Setup(x => x.AdrTemplateSectionWriteRepository).Returns(new AdrTemplateSectionWriteRepository(writerContext));
        adrUnitOfWorkMock.Setup(x => x.AdrVoteReadRepository).Returns(new AdrVoteReadRepository(reader));
        adrUnitOfWorkMock.Setup(x => x.AdrVoteWriteRepository).Returns(new AdrVoteWriteRepository(writerContext));
        adrUnitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns((CancellationToken token) => unitOfWork.SaveChangesAsync(token));
        AdrUnitOfWork = adrUnitOfWorkMock.Object;

        UserOrganizationReadRepository = new UserOrganizationReadRepository(reader);
        UserReadRepository = new UserReadRepository(reader);
    }
}
