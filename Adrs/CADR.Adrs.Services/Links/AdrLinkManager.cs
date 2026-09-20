using AutoMapper;
using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Repositories.Contracts.Extensions;
using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Links;
using CADR.Adrs.Services.Resources;
using CADR.Common.Core.Extensions;

namespace CADR.Adrs.Services.Links;

/// <inheritdoc cref="IAdrLinkManager"/>
internal sealed class AdrLinkManager : IAdrLinkManager, IAdrsServiceAnchor
{
    private readonly IAdrsUnitOfWork unitOfWork;
    private readonly IAdrReadRepository adrReadRepository;
    private readonly IAdrLinkReadRepository adrLinkReadRepository;
    private readonly IAdrLinkWriteRepository adrLinkWriteRepository;
    private readonly IUserOrganizationReadRepository userOrganizationReadRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrLinkManager"/>
    /// </summary>
    public AdrLinkManager(IAdrsUnitOfWork adrUnitOfWork, IMapper mapper, IUserOrganizationReadRepository userOrganizationReadRepository)
    {
        unitOfWork = adrUnitOfWork;
        adrReadRepository = adrUnitOfWork.AdrReadRepository;
        adrLinkReadRepository = adrUnitOfWork.AdrLinkReadRepository;
        adrLinkWriteRepository = adrUnitOfWork.AdrLinkWriteRepository;
        this.userOrganizationReadRepository = userOrganizationReadRepository;
        this.mapper = mapper;
    }

    async Task<AdrLinkModel> IAdrLinkManager.CreateAsync(CreateAdrLinkModel model, CancellationToken cancellationToken)
    {
        var sourceAdr = await adrReadRepository.GetActiveByIdAsync(model.SourceAdrId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<Adr>(model.SourceAdrId));

        var targetAdr = await adrReadRepository.GetActiveByIdAsync(model.TargetAdrId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<Adr>(model.TargetAdrId));

        await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.UserId, sourceAdr!.OrganizationId, cancellationToken);

        if (sourceAdr.OrganizationId != targetAdr!.OrganizationId)
        {
            throw new AdrInvalidOperationException(ErrorMessages.CrossOrganizationLinkForbidden);
        }

        var mappedType = mapper.Map<Entities.Enums.AdrLinkType>(model.Type);

        if (await adrLinkReadRepository.IsActiveLinkExistsAsync(model.SourceAdrId, model.TargetAdrId, mappedType, cancellationToken))
        {
            throw new AdrInvalidOperationException(ErrorMessages.LinkAlreadyExist);
        }

        var link = new AdrLink
        {
            Id = Guid.NewGuid(),
            Type = mappedType,
            SourceAdrId = model.SourceAdrId,
            TargetAdrId = model.TargetAdrId,
        };
        adrLinkWriteRepository.Add(link);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<AdrLinkModel>(link);
        result.TargetAdrNumber = targetAdr.Number;
        result.TargetAdrName = targetAdr.Title;
        return result;
    }

    async Task<IEnumerable<AdrLinkModel>> IAdrLinkManager.GetByAdrIdAsync(Guid adrId, Guid userId, CancellationToken cancellationToken)
    {
        var adr = await adrReadRepository.GetActiveByIdAsync(adrId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<Adr>(adrId));
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, adr!.OrganizationId, cancellationToken);

        var sourceLinks = await adrLinkReadRepository.GetBySourceAdrIdAsync(adrId, cancellationToken);
        var targetLinks = await adrLinkReadRepository.GetByTargetAdrIdAsync(adrId, cancellationToken);
        var result = sourceLinks.Concat(targetLinks)
            .Select(mapper.Map<AdrLinkModel>)
            .ToReadOnlyCollection();
        await FillTargetAdrsAsync(result, cancellationToken);
        return result;
    }

    async Task IAdrLinkManager.DeleteAsync(DeleteAdrLinkModel model, CancellationToken cancellationToken)
    {
        var link = await adrLinkReadRepository.GetActiveByIdAsync(model.AdrLinkId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrLink>(model.AdrLinkId));
        var sourceAdr = await adrReadRepository.GetActiveByIdAsync(link!.SourceAdrId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<Adr>(link.SourceAdrId));
        await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.UserId, sourceAdr!.OrganizationId, cancellationToken);

        adrLinkWriteRepository.Delete(link);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task FillTargetAdrsAsync(IEnumerable<AdrLinkModel> models, CancellationToken cancellationToken)
    {
        foreach (var targetAdrId in models.Select(x => x.TargetAdrId).Distinct())
        {
            var adr = await adrReadRepository.GetActiveByIdAsync(targetAdrId, cancellationToken);
            if (adr == null)
            {
                continue;
            }

            foreach (var model in models.Where(x => x.TargetAdrId == targetAdrId))
            {
                model.TargetAdrNumber = adr.Number;
                model.TargetAdrName = adr.Title;
            }
        }
    }
}
