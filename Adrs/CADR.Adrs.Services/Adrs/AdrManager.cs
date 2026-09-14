using AutoMapper;
using CADR.Administrations.Entities.Enums;
using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Repositories.Contracts.Extensions;
using CADR.Adrs.Entities;
using CADR.Adrs.Entities.Enums;
using CADR.Adrs.Repositories.Contracts;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Adrs;
using CADR.Adrs.Services.Resources;
using CADR.Common.Core.Extensions;

namespace CADR.Adrs.Services.Adrs;

/// <inheritdoc cref="IAdrManager"/>
internal sealed class AdrManager : IAdrManager, IAdrsServiceAnchor
{
    private const int DefaultLikesRequiredForApproval = 1;
    private readonly IAdrUnitOfWork unitOfWork;
    private readonly IAdrReadRepository adrReadRepository;
    private readonly IAdrWriteRepository adrWriteRepository;
    private readonly IAdrSectionReadRepository adrSectionReadRepository;
    private readonly IAdrSectionWriteRepository adrSectionWriteRepository;
    private readonly IAdrVoteReadRepository adrVoteReadRepository;
    private readonly IAdrVoteWriteRepository adrVoteWriteRepository;
    private readonly IAdrFolderReadRepository adrFolderReadRepository;
    private readonly IAdrTemplateReadRepository adrTemplateReadRepository;
    private readonly IAdrTemplateSectionReadRepository adrTemplateSectionReadRepository;
    private readonly IAdrOrganizationSettingsReadRepository adrOrganizationSettingsReadRepository;
    private readonly IUserOrganizationReadRepository userOrganizationReadRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrManager"/>
    /// </summary>
    public AdrManager(IAdrUnitOfWork adrUnitOfWork, IMapper mapper, IUserOrganizationReadRepository userOrganizationReadRepository)
    {
        unitOfWork = adrUnitOfWork;
        adrReadRepository = adrUnitOfWork.AdrReadRepository;
        adrWriteRepository = adrUnitOfWork.AdrWriteRepository;
        adrSectionReadRepository = adrUnitOfWork.AdrSectionReadRepository;
        adrSectionWriteRepository = adrUnitOfWork.AdrSectionWriteRepository;
        adrVoteReadRepository = adrUnitOfWork.AdrVoteReadRepository;
        adrVoteWriteRepository = adrUnitOfWork.AdrVoteWriteRepository;
        adrFolderReadRepository = adrUnitOfWork.AdrFolderReadRepository;
        adrTemplateReadRepository = adrUnitOfWork.AdrTemplateReadRepository;
        adrTemplateSectionReadRepository = adrUnitOfWork.AdrTemplateSectionReadRepository;
        adrOrganizationSettingsReadRepository = adrUnitOfWork.AdrOrganizationSettingsReadRepository;
        this.userOrganizationReadRepository = userOrganizationReadRepository;
        this.mapper = mapper;
    }

    async Task<AdrModel> IAdrManager.CreateAdrAsync(CreateAdrModel model, CancellationToken cancellationToken)
    {
        await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.AuthorId, model.OrganizationId, cancellationToken);
        if (model.ParentAdrFolderId.HasValue)
        {
            await EnsureFolderAsync(model.OrganizationId, model.ParentAdrFolderId.Value, cancellationToken);
        }

        var status = mapper.Map<AdrStatus>(model.Status);
        if (status == AdrStatus.Proposed
            && await GetApprovalThresholdAsync(model.OrganizationId, cancellationToken) == 0)
        {
            status = AdrStatus.Approved;
        }

        var adr = new Adr
        {
            Id = Guid.NewGuid(),
            Number = await adrReadRepository.GetMaxNumberAsync(model.OrganizationId, cancellationToken) + 1,
            Title = model.Title,
            Status = status,
            OrganizationId = model.OrganizationId,
            AuthorId = model.AuthorId,
            ParentAdrFolderId = model.ParentAdrFolderId,
            TemplateId = model.TemplateId,
        };

        if (model.TemplateId.HasValue)
        {
            var template = await adrTemplateReadRepository.GetActiveByIdAsync(model.TemplateId.Value, cancellationToken)
                .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrTemplate>(model.TemplateId.Value));
            if (template!.OrganizationId.HasValue && template.OrganizationId.Value != model.OrganizationId)
            {
                throw new AdrInvalidOperationException(ErrorMessages.TemplateNotAvailable);
            }

            var templateSections = await adrTemplateSectionReadRepository.GetByTemplateIdAsync(template.Id, cancellationToken);
            foreach (var templateSection in templateSections.OrderBy(x => x.Position))
            {
                adr.Sections.Add(new AdrSection
                {
                    Id = Guid.NewGuid(),
                    Position = templateSection.Position,
                    Title = templateSection.Title,
                    Content = templateSection.Placeholder,
                    AdrId = adr.Id,
                });
            }
        }

        adrWriteRepository.Add(adr);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<AdrModel>(adr);
        await FillSectionsAsync(result, cancellationToken);
        await FillComputedAsync(result, model.AuthorId, cancellationToken);
        return result;
    }

    async Task<AdrModel> IAdrManager.GetByIdAsync(Guid adrId, Guid userId, CancellationToken cancellationToken)
    {
        var adr = await GetAdrOrThrowAsync(adrId, cancellationToken);
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, adr.OrganizationId, cancellationToken);

        var result = mapper.Map<AdrModel>(adr);
        await FillSectionsAsync(result, cancellationToken);
        await FillComputedAsync(result, userId, cancellationToken);
        return result;
    }

    async Task<IEnumerable<AdrModel>> IAdrManager.GetByOrganizationIdAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, organizationId, cancellationToken);
        var adrs = await adrReadRepository.GetByOrganizationIdAsync(organizationId, cancellationToken);
        return await MapListAsync(adrs, userId, cancellationToken);
    }

    async Task<IEnumerable<AdrModel>> IAdrManager.GetByFolderIdAsync(Guid organizationId, Guid? folderId, Guid userId, CancellationToken cancellationToken)
    {
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, organizationId, cancellationToken);
        var adrs = await adrReadRepository.GetByFolderIdAsync(organizationId, folderId, cancellationToken);
        return await MapListAsync(adrs, userId, cancellationToken);
    }

    async Task<IEnumerable<AdrModel>> IAdrManager.GetByAuthorIdAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, organizationId, cancellationToken);
        var adrs = await adrReadRepository.GetByAuthorIdAsync(organizationId, userId, cancellationToken);
        return await MapListAsync(adrs, userId, cancellationToken);
    }

    async Task<AdrModel> IAdrManager.UpdateAdrAsync(UpdateAdrModel model, CancellationToken cancellationToken)
    {
        var adr = await GetAdrOrThrowAsync(model.Id, cancellationToken);
        await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.UserId, adr!.OrganizationId, cancellationToken);

        adr.Title = model.Title;
        if (adr.ParentAdrFolderId != model.ParentAdrFolderId && model.ParentAdrFolderId.HasValue)
        {
            await EnsureFolderAsync(adr.OrganizationId, model.ParentAdrFolderId.Value, cancellationToken);
        }

        adr.ParentAdrFolderId = model.ParentAdrFolderId;
        if (adr.Status == AdrStatus.NeedsRevision)
        {
            adr.Status = AdrStatus.Proposed;
            await ApplyInstantApprovalAsync(adr.OrganizationId, newStatus => adr.Status = newStatus, cancellationToken);
        }

        var existingSections = await adrSectionReadRepository.GetByAdrIdAsync(adr.Id, cancellationToken);
        foreach (var section in existingSections)
        {
            adrSectionWriteRepository.Delete(section);
        }

        foreach (var sectionModel in model.Sections.OrderBy(x => x.Position))
        {
            adrSectionWriteRepository.Add(new AdrSection
            {
                Id = Guid.NewGuid(),
                Position = sectionModel.Position,
                Title = sectionModel.Title,
                Content = sectionModel.Content,
                AdrId = adr.Id,
            });
        }

        adrWriteRepository.Update(adr);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<AdrModel>(adr);
        await FillSectionsAsync(result, cancellationToken);
        await FillComputedAsync(result, model.UserId, cancellationToken);
        return result;
    }

    async Task IAdrManager.DeleteAdrAsync(DeleteAdrModel model, CancellationToken cancellationToken)
    {
        var adr = await GetAdrOrThrowAsync(model.AdrId, cancellationToken);
        await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.UserId, adr.OrganizationId, cancellationToken);

        var sections = await adrSectionReadRepository.GetByAdrIdAsync(adr.Id, cancellationToken);
        foreach (var section in sections)
        {
            adrSectionWriteRepository.Delete(section);
        }

        adrWriteRepository.Delete(adr);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    async Task IAdrManager.ChangeStatusAsync(ChangeAdrStatusModel model, CancellationToken cancellationToken)
    {
        var adr = await GetAdrOrThrowAsync(model.AdrId, cancellationToken);
        var userOrganization = await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(model.UserId, adr!.OrganizationId, cancellationToken)
            .OrThrowIfNull(() => new AdrAccessException());
        var current = adr.Status;
        var target = mapper.Map<AdrStatus>(model.Status);
        if (!IsTransitionAllowed(current, target, userOrganization!.Role, adr.AuthorId == model.UserId))
        {
            throw new AdrInvalidOperationException(string.Format(ErrorMessages.InvalidStatusTransition, current, target));
        }

        adr.Status = target;
        await ApplyInstantApprovalAsync(adr.OrganizationId, newStatus => adr.Status = newStatus, cancellationToken);
        adrWriteRepository.Update(adr);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    async Task IAdrManager.VoteAsync(VoteAdrModel model, CancellationToken cancellationToken)
    {
        var adr = await GetAdrOrThrowAsync(model.AdrId, cancellationToken);
        await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.UserId, adr.OrganizationId, cancellationToken);
        if (adr.AuthorId == model.UserId)
        {
            throw new AdrInvalidOperationException(ErrorMessages.CannotVoteOwnAdr);
        }

        var vote = await adrVoteReadRepository.GetByAdrAndUserIdAsync(adr.Id, model.UserId, cancellationToken);
        if (vote == null)
        {
            vote = new AdrVote
            {
                Id = Guid.NewGuid(),
                AdrId = adr.Id,
                UserId = model.UserId,
                Value = mapper.Map<AdrVoteType>(model.Vote),
            };
            adrVoteWriteRepository.Add(vote);
        }
        else
        {
            vote.Value = mapper.Map<AdrVoteType>(model.Vote);
            adrVoteWriteRepository.Update(vote);
        }

        await ApplyVoteStatusAsync(adr, cancellationToken);
        adrWriteRepository.Update(adr);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    async Task IAdrManager.WithdrawVoteAsync(WithdrawVoteAdrModel model, CancellationToken cancellationToken)
    {
        var adr = await GetAdrOrThrowAsync(model.AdrId, cancellationToken);
        await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.UserId, adr!.OrganizationId, cancellationToken);

        var vote = await adrVoteReadRepository.GetByAdrAndUserIdAsync(adr.Id, model.UserId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrVote>(adr.Id));
        adrVoteWriteRepository.Delete(vote!);

        await ApplyVoteStatusAsync(adr, cancellationToken);
        adrWriteRepository.Update(adr);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Adr> GetAdrOrThrowAsync(Guid adrId, CancellationToken cancellationToken)
        => (await adrReadRepository.GetActiveByIdAsync(adrId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<Adr>(adrId)))!;

    private async Task EnsureFolderAsync(Guid organizationId, Guid folderId, CancellationToken cancellationToken)
    {
        var folder = await adrFolderReadRepository.GetActiveByIdAsync(folderId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrFolder>(folderId));
        if (folder!.OrganizationId != organizationId)
        {
            throw new AdrInvalidOperationException(ErrorMessages.ParentFolderFromAnotherOrganization);
        }
    }

    private async Task ApplyInstantApprovalAsync(Guid organizationId, Action<AdrStatus> setStatus, CancellationToken cancellationToken)
    {
        var settings = await adrOrganizationSettingsReadRepository.GetByOrganizationIdAsync(organizationId, cancellationToken);
        var threshold = settings?.LikesRequiredForApproval ?? DefaultLikesRequiredForApproval;
        if (threshold == 0)
        {
            setStatus(AdrStatus.Approved);
        }
    }

    private async Task ApplyVoteStatusAsync(Adr adr, CancellationToken cancellationToken)
    {
        var score = await ComputeScoreAsync(adr.Id, cancellationToken);
        if (score < 0 && adr.Status is AdrStatus.Approved or AdrStatus.NeedsRevision)
        {
            adr.Status = AdrStatus.NeedsRevision;
        }
        else if (adr.Status == AdrStatus.NeedsRevision && score >= 0)
        {
            adr.Status = AdrStatus.Approved;
        }
        else if (adr.Status == AdrStatus.Proposed && score >= await GetApprovalThresholdAsync(adr.OrganizationId, cancellationToken))
        {
            adr.Status = AdrStatus.Approved;
        }
    }

    private async Task<int> GetApprovalThresholdAsync(Guid organizationId, CancellationToken cancellationToken)
    {
        var settings = await adrOrganizationSettingsReadRepository.GetByOrganizationIdAsync(organizationId, cancellationToken);
        return settings?.LikesRequiredForApproval ?? DefaultLikesRequiredForApproval;
    }

    private async Task<int> ComputeScoreAsync(Guid adrId, CancellationToken cancellationToken)
    {
        var votes = await adrVoteReadRepository.GetByAdrIdAsync(adrId, cancellationToken);
        return votes.Sum(x => x.Value == AdrVoteType.Like ? 1 : -1);
    }

    private async Task<IReadOnlyCollection<AdrModel>> MapListAsync(IReadOnlyCollection<Adr> adrs, Guid userId, CancellationToken cancellationToken)
    {
        var result = mapper.Map<IEnumerable<AdrModel>>(adrs).ToReadOnlyCollection();
        foreach (var model in result)
        {
            await FillComputedAsync(model, userId, cancellationToken);
        }

        return result;
    }

    private async Task FillSectionsAsync(AdrModel model, CancellationToken cancellationToken)
    {
        var sections = await adrSectionReadRepository.GetByAdrIdAsync(model.Id, cancellationToken);
        model.Sections = mapper.Map<ICollection<AdrSectionModel>>(sections.OrderBy(x => x.Position).ToReadOnlyCollection());
    }

    private async Task FillComputedAsync(AdrModel model, Guid userId, CancellationToken cancellationToken)
    {
        var votes = await adrVoteReadRepository.GetByAdrIdAsync(model.Id, cancellationToken);
        model.Score = votes.Sum(x => x.Value == AdrVoteType.Like ? 1 : -1);
        var userVote = votes.FirstOrDefault(x => x.UserId == userId);
        model.UserVote = userVote == null ? null : mapper.Map<CADR.Adrs.Services.Contracts.Models.Enums.AdrVoteType>(userVote.Value);
    }

    private static bool IsTransitionAllowed(AdrStatus current, AdrStatus target, Role role, bool isAuthor)
        => (current, target) switch
        {
            (AdrStatus.Draft, AdrStatus.Proposed) => role is Role.Admin or Role.Architect,
            (AdrStatus.Proposed, AdrStatus.Approved) => role == Role.Admin,
            (AdrStatus.Proposed, AdrStatus.Rejected) => role == Role.Admin,
            (AdrStatus.Approved, AdrStatus.NeedsRevision) => role == Role.Admin,
            (AdrStatus.Approved, AdrStatus.Deprecated) => role == Role.Admin || isAuthor,
            (AdrStatus.NeedsRevision, AdrStatus.Approved) => role == Role.Admin,
            (AdrStatus.NeedsRevision, AdrStatus.Proposed) => role is Role.Admin or Role.Architect,
            (AdrStatus.NeedsRevision, AdrStatus.Deprecated) => role == Role.Admin || isAuthor,
            (AdrStatus.Rejected, AdrStatus.Proposed) => role == Role.Admin,
            _ => false,
        };
}
