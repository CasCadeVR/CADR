using System.Text;
using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Repositories.Contracts.Extensions;
using CADR.Adrs.Entities;
using CADR.Adrs.Entities.Enums;
using CADR.Adrs.Repositories.Contracts;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Common.Core.Extensions;

namespace CADR.Adrs.Services.Export;

/// <inheritdoc cref="IAdrExportService"/>
internal sealed class AdrExportService : IAdrExportService, IAdrsServiceAnchor
{
    private readonly IAdrReadRepository adrReadRepository;
    private readonly IAdrSectionReadRepository adrSectionReadRepository;
    private readonly IUserOrganizationReadRepository userOrganizationReadRepository;
    private readonly IUserReadRepository userReadRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrExportService"/>
    /// </summary>
    public AdrExportService(IAdrsUnitOfWork adrUnitOfWork, IUserOrganizationReadRepository userOrganizationReadRepository, IUserReadRepository userReadRepository)
    {
        adrReadRepository = adrUnitOfWork.AdrReadRepository;
        adrSectionReadRepository = adrUnitOfWork.AdrSectionReadRepository;
        this.userOrganizationReadRepository = userOrganizationReadRepository;
        this.userReadRepository = userReadRepository;
    }

    async Task<string> IAdrExportService.ExportToMarkdownAsync(Guid adrId, Guid userId, CancellationToken cancellationToken)
    {
        var adr = await adrReadRepository.GetActiveByIdAsync(adrId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<Adr>(adrId));
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, adr!.OrganizationId, cancellationToken);

        return await RenderAdrMarkdownAsync(adr, cancellationToken);
    }

    async Task<string> IAdrExportService.ExportApprovedToMarkdownAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, organizationId, cancellationToken);
        var adrs = await adrReadRepository.GetByStatusesAsync(organizationId, [AdrStatus.Approved], cancellationToken);

        var documents = new List<string>(adrs.Count);
        foreach (var adr in adrs.OrderBy(x => x.Number))
        {
            documents.Add(await RenderAdrMarkdownAsync(adr, cancellationToken));
        }

        return string.Join($"{Environment.NewLine}---{Environment.NewLine}", documents);
    }

    private async Task<string> RenderAdrMarkdownAsync(Adr adr, CancellationToken cancellationToken)
    {
        var sections = await adrSectionReadRepository.GetByAdrIdAsync(adr.Id, cancellationToken);
        var author = await userReadRepository.GetByIdAsync(adr.AuthorId, cancellationToken);

        var builder = new StringBuilder();
        builder.AppendLine($"# {adr.Number}. {adr.Title}");
        builder.AppendLine();
        builder.AppendLine($"**Статус:** {adr.Status}");
        if (author != null)
        {
            builder.AppendLine($"**Автор:** {author.Name}");
        }

        builder.AppendLine($"**Дата создания:** {adr.CreatedAt.UtcDateTime:d}");
        foreach (var section in sections.OrderBy(x => x.Position))
        {
            builder.AppendLine();
            builder.AppendLine($"## {section.Title}");
            builder.AppendLine();
            builder.AppendLine(section.Content);
        }

        return builder.ToString();
    }
}
