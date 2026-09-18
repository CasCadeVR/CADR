using CADR.Adrs.Repositories.Contracts;
using CADR.Context.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace CADR.Adrs.Repositories;

/// <inheritdoc cref="IAdrsUnitOfWork"/>
internal class AdrsUnitOfWork : IAdrsUnitOfWork, IAdrsRepositoryAnchor
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IServiceProvider serviceProvider;

    public AdrsUnitOfWork(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        this.unitOfWork = unitOfWork;
        this.serviceProvider = serviceProvider;
    }

    Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
        => unitOfWork.SaveChangesAsync(cancellationToken);

    IAdrCommentReadRepository IAdrsUnitOfWork.AdrCommentReadRepository
        => serviceProvider.GetRequiredService<IAdrCommentReadRepository>();

    IAdrCommentWriteRepository IAdrsUnitOfWork.AdrCommentWriteRepository
        => serviceProvider.GetRequiredService<IAdrCommentWriteRepository>();

    IAdrFolderReadRepository IAdrsUnitOfWork.AdrFolderReadRepository
        => serviceProvider.GetRequiredService<IAdrFolderReadRepository>();

    IAdrFolderWriteRepository IAdrsUnitOfWork.AdrFolderWriteRepository
        => serviceProvider.GetRequiredService<IAdrFolderWriteRepository>();

    IAdrLinkReadRepository IAdrsUnitOfWork.AdrLinkReadRepository
        => serviceProvider.GetRequiredService<IAdrLinkReadRepository>();

    IAdrLinkWriteRepository IAdrsUnitOfWork.AdrLinkWriteRepository
        => serviceProvider.GetRequiredService<IAdrLinkWriteRepository>();

    IAdrOrganizationSettingsReadRepository IAdrsUnitOfWork.AdrOrganizationSettingsReadRepository
        => serviceProvider.GetRequiredService<IAdrOrganizationSettingsReadRepository>();

    IAdrOrganizationSettingsWriteRepository IAdrsUnitOfWork.AdrOrganizationSettingsWriteRepository
        => serviceProvider.GetRequiredService<IAdrOrganizationSettingsWriteRepository>();

    IAdrReadRepository IAdrsUnitOfWork.AdrReadRepository
        => serviceProvider.GetRequiredService<IAdrReadRepository>();

    IAdrWriteRepository IAdrsUnitOfWork.AdrWriteRepository
        => serviceProvider.GetRequiredService<IAdrWriteRepository>();

    IAdrSectionReadRepository IAdrsUnitOfWork.AdrSectionReadRepository
        => serviceProvider.GetRequiredService<IAdrSectionReadRepository>();

    IAdrSectionWriteRepository IAdrsUnitOfWork.AdrSectionWriteRepository
        => serviceProvider.GetRequiredService<IAdrSectionWriteRepository>();

    IAdrTemplateReadRepository IAdrsUnitOfWork.AdrTemplateReadRepository
        => serviceProvider.GetRequiredService<IAdrTemplateReadRepository>();

    IAdrTemplateWriteRepository IAdrsUnitOfWork.AdrTemplateWriteRepository
        => serviceProvider.GetRequiredService<IAdrTemplateWriteRepository>();

    IAdrTemplateSectionReadRepository IAdrsUnitOfWork.AdrTemplateSectionReadRepository
        => serviceProvider.GetRequiredService<IAdrTemplateSectionReadRepository>();

    IAdrTemplateSectionWriteRepository IAdrsUnitOfWork.AdrTemplateSectionWriteRepository
        => serviceProvider.GetRequiredService<IAdrTemplateSectionWriteRepository>();

    IAdrVoteReadRepository IAdrsUnitOfWork.AdrVoteReadRepository
        => serviceProvider.GetRequiredService<IAdrVoteReadRepository>();

    IAdrVoteWriteRepository IAdrsUnitOfWork.AdrVoteWriteRepository
        => serviceProvider.GetRequiredService<IAdrVoteWriteRepository>();
}

