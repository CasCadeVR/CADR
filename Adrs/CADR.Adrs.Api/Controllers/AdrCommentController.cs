using AutoMapper;
using CADR.Adrs.Api.Models.Comments;
using CADR.Adrs.Api.Resources;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Comments;
using CADR.Common.Core.Contracts;
using CADR.Common.Mvc.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CADR.Adrs.Api.Controllers;

/// <summary>
/// Работа с комментариями ADR
/// </summary>
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = $"{AdrsConstants.DocPrefix}v1")]
[Route(AdrsConstants.DefaultControllerRoute)]
public class AdrCommentController : ControllerBase
{
    private readonly IAdrCommentManager adrCommentManager;
    private readonly IAdrValidateService adrValidateService;
    private readonly IIdentityProvider identityProvider;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrCommentController"/>
    /// </summary>
    public AdrCommentController(IAdrCommentManager adrCommentManager,
        IAdrValidateService adrValidateService,
        IIdentityProvider identityProvider,
        IMapper mapper)
    {
        this.adrCommentManager = adrCommentManager;
        this.adrValidateService = adrValidateService;
        this.identityProvider = identityProvider;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получает список комментариев ADR
    /// </summary>
    [HttpGet("{adrId:guid}")]
    [ApiOk(typeof(IEnumerable<AdrCommentApiModel>))]
    [ApiUnauthorized]
    [ApiForbidden]
    [SwaggerOperation(OperationId = "AdrCommentGetByAdr")]
    public async Task<IActionResult> GetByAdr(Guid adrId, CancellationToken cancellationToken)
    {
        var result = await adrCommentManager.GetByAdrIdAsync(adrId, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<IEnumerable<AdrCommentApiModel>>(result));
    }

    /// <summary>
    /// Создаёт новый комментарий ADR
    /// </summary>
    [HttpPost]
    [ApiOk(typeof(AdrCommentApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [ApiValidation]
    [SwaggerOperation(OperationId = "AdrCommentCreate")]
    public async Task<IActionResult> Create(CreateAdrCommentApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<CreateAdrCommentModel>(request);
        model.UserId = identityProvider.Id;
        await adrValidateService.ValidateAsync(model, cancellationToken);
        var result = await adrCommentManager.CreateAsync(model, cancellationToken);
        return Ok(mapper.Map<AdrCommentApiModel>(result));
    }

    /// <summary>
    /// Обновляет существующий комментарий ADR
    /// </summary>
    [HttpPut]
    [ApiOk(typeof(AdrCommentApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [ApiValidation]
    [SwaggerOperation(OperationId = "AdrCommentUpdate")]
    public async Task<IActionResult> Update(UpdateAdrCommentApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<UpdateAdrCommentModel>(request);
        model.UserId = identityProvider.Id;
        await adrValidateService.ValidateAsync(model, cancellationToken);
        var result = await adrCommentManager.UpdateAsync(model, cancellationToken);
        return Ok(mapper.Map<AdrCommentApiModel>(result));
    }

    /// <summary>
    /// Удаляет существующий комментарий ADR
    /// </summary>
    [HttpDelete("{adrCommentId:guid}")]
    [ApiNoContent]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [SwaggerOperation(OperationId = "AdrCommentDelete")]
    public async Task<IActionResult> Delete(Guid adrCommentId, CancellationToken cancellationToken)
    {
        var model = new DeleteAdrCommentModel
        {
            UserId = identityProvider.Id,
            AdrCommentId = adrCommentId,
        };
        await adrCommentManager.DeleteAsync(model, cancellationToken);
        return NoContent();
    }
}
