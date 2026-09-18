using AutoMapper;
using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Repositories.Contracts.Extensions;
using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Settings;

namespace CADR.Adrs.Services.Settings;

/// <inheritdoc cref="IAdrOrganizationSettingsManager"/>
internal sealed class AdrOrganizationSettingsManager : IAdrOrganizationSettingsManager, IAdrsServiceAnchor
{
    private const int DefaultLikesRequiredForApproval = 1;
    private readonly IAdrsUnitOfWork unitOfWork;
    private readonly IAdrOrganizationSettingsReadRepository adrOrganizationSettingsReadRepository;
    private readonly IAdrOrganizationSettingsWriteRepository adrOrganizationSettingsWriteRepository;
    private readonly IUserOrganizationReadRepository userOrganizationReadRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrOrganizationSettingsManager"/>
    /// </summary>
    public AdrOrganizationSettingsManager(IAdrsUnitOfWork adrUnitOfWork, IMapper mapper, IUserOrganizationReadRepository userOrganizationReadRepository)
    {
        unitOfWork = adrUnitOfWork;
        adrOrganizationSettingsReadRepository = adrUnitOfWork.AdrOrganizationSettingsReadRepository;
        adrOrganizationSettingsWriteRepository = adrUnitOfWork.AdrOrganizationSettingsWriteRepository;
        this.userOrganizationReadRepository = userOrganizationReadRepository;
        this.mapper = mapper;
    }

    async Task<AdrOrganizationSettingsModel> IAdrOrganizationSettingsManager.GetByOrganizationIdAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, organizationId, cancellationToken);
        var settings = await adrOrganizationSettingsReadRepository.GetByOrganizationIdAsync(organizationId, cancellationToken);
        if (settings == null)
        {
            return new AdrOrganizationSettingsModel
            {
                OrganizationId = organizationId,
                LikesRequiredForApproval = DefaultLikesRequiredForApproval,
            };
        }

        return mapper.Map<AdrOrganizationSettingsModel>(settings);
    }

    async Task<AdrOrganizationSettingsModel> IAdrOrganizationSettingsManager.UpdateAsync(UpdateAdrOrganizationSettingsModel model, CancellationToken cancellationToken)
    {
        await userOrganizationReadRepository.ThrowIfNotAdminAsync<AdrAccessException>(model.UserId, model.OrganizationId, cancellationToken);
        var settings = await adrOrganizationSettingsReadRepository.GetByOrganizationIdAsync(model.OrganizationId, cancellationToken);
        if (settings == null)
        {
            settings = new AdrOrganizationSettings
            {
                Id = Guid.NewGuid(),
                OrganizationId = model.OrganizationId,
                LikesRequiredForApproval = model.LikesRequiredForApproval,
            };
            adrOrganizationSettingsWriteRepository.Add(settings);
        }
        else
        {
            settings.LikesRequiredForApproval = model.LikesRequiredForApproval;
            adrOrganizationSettingsWriteRepository.Update(settings);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<AdrOrganizationSettingsModel>(settings);
    }
}
