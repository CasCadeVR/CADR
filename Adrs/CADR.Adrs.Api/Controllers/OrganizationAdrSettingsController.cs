using AutoMapper;
using CADR.Adrs.Api.Models.Settings;
using CADR.Adrs.Api.Resources;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Settings;
using CADR.Common.Core.Contracts;
using CADR.Common.Mvc.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CADR.Adrs.Api.Controllers;

/// <summary>
/// Работа с настройками ADR организации
/// </summary>
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = $"{AdrsConstants.DocPrefix}v1")]
[Route(AdrsConstants.DefaultControllerRoute)]
public class OrganizationAdrSettingsController : ControllerBase
{
    private readonly IAdrOrganizationSettingsManager adrOrganizationSettingsManager;
    private readonly IAdrValidateService validateService;
    private readonly IIdentityProvider identityProvider;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationAdrSettingsController"/>
    /// </summary>
    public OrganizationAdrSettingsController(IAdrOrganizationSettingsManager adrOrganizationSettingsManager,
        IAdrValidateService validateService,
        IIdentityProvider identityProvider,
        IMapper mapper)
    {
        this.adrOrganizationSettingsManager = adrOrganizationSettingsManager;
        this.validateService = validateService;
        this.identityProvider = identityProvider;
        this.mapper = mapper;
    }

    /// <summary>
    /// Сохраняет настройки ADR
    /// </summary>
    [HttpPost]
    [ApiOk(typeof(AdrOrganizationSettingsApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiValidation]
    [ApiBad]
    [SwaggerOperation(OperationId = "AdrOrganizationSettingsSave")]
    public async Task<IActionResult> Save(UpdateAdrOrganizationSettingsApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<UpdateAdrOrganizationSettingsModel>(request);
        model.UserId = identityProvider.Id;
        await validateService.ValidateAsync(model, cancellationToken);
        var result = await adrOrganizationSettingsManager.UpdateAsync(model, cancellationToken);
        return Ok(mapper.Map<AdrOrganizationSettingsApiModel>(result));
    }

    /// <summary>
    /// Получает настройки ADR по идентификатору организации
    /// </summary>
    [HttpGet("{organizationId:guid}")]
    [ApiOk(typeof(AdrOrganizationSettingsApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [SwaggerOperation(OperationId = "AdrOrganizationSettingsGetByOrganizationId")]
    public async Task<IActionResult> GetAdrSettingsByOrganizationId(Guid organizationId, CancellationToken cancellationToken)
    {
        var result = await adrOrganizationSettingsManager.GetByOrganizationIdAsync(organizationId, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<AdrOrganizationSettingsApiModel>(result));
    }
}
