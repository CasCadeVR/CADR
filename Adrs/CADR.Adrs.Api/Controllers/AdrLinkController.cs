using AutoMapper;
using CADR.Adrs.Api.Models.Links;
using CADR.Adrs.Api.Resources;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Links;
using CADR.Common.Core.Contracts;
using CADR.Common.Mvc.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CADR.Adrs.Api.Controllers;

/// <summary>
/// Работа со связями ADR
/// </summary>
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = $"{AdrsConstants.DocPrefix}v1")]
[Route(AdrsConstants.DefaultControllerRoute)]
public class AdrLinkController : ControllerBase
{
    private readonly IAdrLinkManager adrLinkManager;
    private readonly IAdrValidateService adrValidateService;
    private readonly IIdentityProvider identityProvider;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrLinkController"/>
    /// </summary>
    public AdrLinkController(IAdrLinkManager adrLinkManager,
        IAdrValidateService adrValidateService,
        IIdentityProvider identityProvider,
        IMapper mapper)
    {
        this.adrLinkManager = adrLinkManager;
        this.adrValidateService = adrValidateService;
        this.identityProvider = identityProvider;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получает список всех связей ADR (в обе стороны)
    /// </summary>
    [HttpGet("{adrId:guid}")]
    [ApiOk(typeof(IEnumerable<AdrLinkApiModel>))]
    [ApiUnauthorized]
    [ApiForbidden]
    [SwaggerOperation(OperationId = "AdrLinkGetByAdr")]
    public async Task<IActionResult> GetByAdr(Guid adrId, CancellationToken cancellationToken)
    {
        var result = await adrLinkManager.GetByAdrIdAsync(adrId, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<IEnumerable<AdrLinkApiModel>>(result));
    }

    /// <summary>
    /// Создаёт новую связь между ADR
    /// </summary>
    [HttpPost]
    [ApiOk(typeof(AdrLinkApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [ApiValidation]
    [ApiBad]
    [SwaggerOperation(OperationId = "AdrLinkCreate")]
    public async Task<IActionResult> Create(CreateAdrLinkApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<CreateAdrLinkModel>(request);
        model.UserId = identityProvider.Id;
        await adrValidateService.ValidateAsync(model, cancellationToken);
        var result = await adrLinkManager.CreateAsync(model, cancellationToken);
        return Ok(mapper.Map<AdrLinkApiModel>(result));
    }

    /// <summary>
    /// Удаляет существующую связь ADR
    /// </summary>
    [HttpDelete("{adrLinkId:guid}")]
    [ApiNoContent]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [SwaggerOperation(OperationId = "AdrLinkDelete")]
    public async Task<IActionResult> Delete(Guid adrLinkId, CancellationToken cancellationToken)
    {
        var model = new DeleteAdrLinkModel
        {
            UserId = identityProvider.Id,
            AdrLinkId = adrLinkId,
        };
        await adrLinkManager.DeleteAsync(model, cancellationToken);
        return NoContent();
    }
}
