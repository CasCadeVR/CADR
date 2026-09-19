using AutoMapper;
using CADR.Adrs.Api.Models.Templates;
using CADR.Adrs.Api.Resources;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Templates;
using CADR.Common.Core.Contracts;
using CADR.Common.Mvc.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CADR.Adrs.Api.Controllers;

/// <summary>
/// Работа с шаблонами ADR
/// </summary>
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = $"{AdrsConstants.DocPrefix}v1")]
[Route(AdrsConstants.DefaultControllerRoute)]
public class AdrTemplateController : ControllerBase
{
    private readonly IAdrTemplateManager adrTemplateManager;
    private readonly IAdrValidateService adrValidateService;
    private readonly IIdentityProvider identityProvider;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrTemplateController"/>
    /// </summary>
    public AdrTemplateController(IAdrTemplateManager adrTemplateManager,
        IAdrValidateService adrValidateService,
        IIdentityProvider identityProvider,
        IMapper mapper)
    {
        this.adrTemplateManager = adrTemplateManager;
        this.adrValidateService = adrValidateService;
        this.identityProvider = identityProvider;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получает шаблон ADR по идентификатору вместе с разделами
    /// </summary>
    [HttpGet("{id:guid}")]
    [ApiOk(typeof(AdrTemplateApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [SwaggerOperation(OperationId = "AdrTemplateGet")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await adrTemplateManager.GetByIdAsync(id, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<AdrTemplateApiModel>(result));
    }

    /// <summary>
    /// Получает список шаблонов, доступных организации (глобальные и собственные)
    /// </summary>
    [HttpGet("organization/{organizationId:guid}")]
    [ApiOk(typeof(IEnumerable<AdrTemplateApiModel>))]
    [ApiUnauthorized]
    [ApiForbidden]
    [SwaggerOperation(OperationId = "AdrTemplateGetAvailableForOrganization")]
    public async Task<IActionResult> GetAvailableForOrganization(Guid organizationId, CancellationToken cancellationToken)
    {
        var result = await adrTemplateManager.GetAvailableForOrganizationAsync(organizationId, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<IEnumerable<AdrTemplateApiModel>>(result));
    }

    /// <summary>
    /// Создаёт новый шаблон ADR организации
    /// </summary>
    [HttpPost]
    [ApiOk(typeof(AdrTemplateApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiValidation]
    [ApiBad]
    [SwaggerOperation(OperationId = "AdrTemplateCreate")]
    public async Task<IActionResult> Create(CreateAdrTemplateApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<CreateAdrTemplateModel>(request);
        model.UserId = identityProvider.Id;
        await adrValidateService.ValidateAsync(model, cancellationToken);
        var result = await adrTemplateManager.CreateAsync(model, cancellationToken);
        return Ok(mapper.Map<AdrTemplateApiModel>(result));
    }

    /// <summary>
    /// Обновляет существующий шаблон ADR организации
    /// </summary>
    [HttpPut]
    [ApiOk(typeof(AdrTemplateApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [ApiValidation]
    [ApiBad]
    [SwaggerOperation(OperationId = "AdrTemplateUpdate")]
    public async Task<IActionResult> Update(UpdateAdrTemplateApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<UpdateAdrTemplateModel>(request);
        model.UserId = identityProvider.Id;
        await adrValidateService.ValidateAsync(model, cancellationToken);
        var result = await adrTemplateManager.UpdateAsync(model, cancellationToken);
        return Ok(mapper.Map<AdrTemplateApiModel>(result));
    }

    /// <summary>
    /// Удаляет существующий шаблон ADR организации
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ApiNoContent]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [SwaggerOperation(OperationId = "AdrTemplateDelete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var model = new DeleteAdrTemplateModel
        {
            UserId = identityProvider.Id,
            AdrTemplateId = id,
        };
        await adrTemplateManager.DeleteAsync(model, cancellationToken);
        return NoContent();
    }
}
