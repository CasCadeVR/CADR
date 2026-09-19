using CADR.Context.Contracts;

namespace CADR.Adrs.Repositories.Contracts;

/// <summary>
/// <see cref="IUnitOfWork"/> для раздела ADR
/// </summary>
public interface IAdrsUnitOfWork : IUnitOfWork
{
    /// <inheritdoc cref="IAdrCommentReadRepository"/>
    IAdrCommentReadRepository AdrCommentReadRepository { get; }

    /// <inheritdoc cref="IAdrCommentWriteRepository"/>
    IAdrCommentWriteRepository AdrCommentWriteRepository { get; }

    /// <inheritdoc cref="IAdrFolderReadRepository"/>
    IAdrFolderReadRepository AdrFolderReadRepository { get; }

    /// <inheritdoc cref="IAdrFolderWriteRepository"/>
    IAdrFolderWriteRepository AdrFolderWriteRepository { get; }

    /// <inheritdoc cref="IAdrLinkReadRepository"/>
    IAdrLinkReadRepository AdrLinkReadRepository { get; }

    /// <inheritdoc cref="IAdrLinkWriteRepository"/>
    IAdrLinkWriteRepository AdrLinkWriteRepository { get; }

    /// <inheritdoc cref="IAdrOrganizationSettingsReadRepository"/>
    IAdrOrganizationSettingsReadRepository AdrOrganizationSettingsReadRepository { get; }

    /// <inheritdoc cref="IAdrOrganizationSettingsWriteRepository"/>
    IAdrOrganizationSettingsWriteRepository AdrOrganizationSettingsWriteRepository { get; }

    /// <inheritdoc cref="IAdrReadRepository"/>
    IAdrReadRepository AdrReadRepository { get; }

    /// <inheritdoc cref="IAdrWriteRepository"/>
    IAdrWriteRepository AdrWriteRepository { get; }

    /// <inheritdoc cref="IAdrSectionReadRepository"/>
    IAdrSectionReadRepository AdrSectionReadRepository { get; }

    /// <inheritdoc cref="IAdrSectionWriteRepository"/>
    IAdrSectionWriteRepository AdrSectionWriteRepository { get; }

    /// <inheritdoc cref="IAdrTemplateReadRepository"/>
    IAdrTemplateReadRepository AdrTemplateReadRepository { get; }

    /// <inheritdoc cref="IAdrTemplateWriteRepository"/>
    IAdrTemplateWriteRepository AdrTemplateWriteRepository { get; }

    /// <inheritdoc cref="IAdrTemplateSectionReadRepository"/>
    IAdrTemplateSectionReadRepository AdrTemplateSectionReadRepository { get; }

    /// <inheritdoc cref="IAdrTemplateSectionWriteRepository"/>
    IAdrTemplateSectionWriteRepository AdrTemplateSectionWriteRepository { get; }

    /// <inheritdoc cref="IAdrVoteReadRepository"/>
    IAdrVoteReadRepository AdrVoteReadRepository { get; }

    /// <inheritdoc cref="IAdrVoteWriteRepository"/>
    IAdrVoteWriteRepository AdrVoteWriteRepository { get; }
}
