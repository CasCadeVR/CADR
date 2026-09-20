using AutoMapper;
using CADR.Adrs.Api.Models.Folders;
using CADR.Adrs.Api.Resources;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Folders;
using CADR.Common.Core.Contracts;
using CADR.Common.Mvc.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CADR.Adrs.Api.Controllers;

/// <summary>
/// Работа с папками ADR
/// </summary>
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = $"{AdrsConstants.DocPrefix}v1")]
[Route(AdrsConstants.DefaultControllerRoute)]
public class AdrFolderController : ControllerBase
{
    private readonly IAdrFolderManager adrFolderManager;
    private readonly IAdrValidateService adrValidateService;
    private readonly IIdentityProvider identityProvider;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrFolderController"/>
    /// </summary>
    public AdrFolderController(IAdrFolderManager adrFolderManager,
        IAdrValidateService adrValidateService,
        IIdentityProvider identityProvider,
        IMapper mapper)
    {
        this.adrFolderManager = adrFolderManager;
        this.adrValidateService = adrValidateService;
        this.identityProvider = identityProvider;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получает список всех папок ADR организации
    /// </summary>
    [HttpGet("organization/{organizationId:guid}")]
    [ApiOk(typeof(IEnumerable<AdrFolderApiModel>))]
    [ApiUnauthorized]
    [ApiForbidden]
    [SwaggerOperation(OperationId = "AdrFolderGetByOrganization")]
    public async Task<IActionResult> GetByOrganization(Guid organizationId, CancellationToken cancellationToken)
    {
        var result = await adrFolderManager.GetByOrganizationIdAsync(organizationId, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<IEnumerable<AdrFolderApiModel>>(result));
    }

    /// <summary>
    /// Получает путь к папке от корня организации (цепочку родительских папок)
    /// </summary>
    [HttpGet("organization/{organizationId:guid}/path/{folderId:guid?}")]
    [ApiOk(typeof(IEnumerable<AdrFolderApiModel>))]
    [ApiUnauthorized]
    [ApiForbidden]
    [SwaggerOperation(OperationId = "AdrFolderGetPath")]
    public async Task<IActionResult> GetPath(Guid organizationId, Guid? folderId, CancellationToken cancellationToken)
    {
        var result = await adrFolderManager.GetPathAsync(organizationId, folderId, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<IEnumerable<AdrFolderApiModel>>(result));
    }

    /// <summary>
    /// Получает число ADR, которое будет удалено вместе с папкой (включая вложенные папки)
    /// </summary>
    [HttpGet("organization/{organizationId:guid}/{folderId:guid}/adr-count")]
    [ApiOk(typeof(int))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [SwaggerOperation(OperationId = "AdrFolderGetAdrCount")]
    public async Task<IActionResult> GetAdrCount(Guid organizationId, Guid folderId, CancellationToken cancellationToken)
    {
        var result = await adrFolderManager.GetAdrCountAsync(organizationId, folderId, identityProvider.Id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Создаёт новую папку ADR
    /// </summary>
    [HttpPost]
    [ApiOk(typeof(AdrFolderApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiValidation]
    [ApiBad]
    [SwaggerOperation(OperationId = "AdrFolderCreate")]
    public async Task<IActionResult> Create(CreateAdrFolderApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<CreateAdrFolderModel>(request);
        model.UserId = identityProvider.Id;
        await adrValidateService.ValidateAsync(model, cancellationToken);
        var result = await adrFolderManager.CreateAsync(model, cancellationToken);
        return Ok(mapper.Map<AdrFolderApiModel>(result));
    }

    /// <summary>
    /// Обновляет существующую папку ADR (переименование, перемещение)
    /// </summary>
    [HttpPut]
    [ApiOk(typeof(AdrFolderApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [ApiValidation]
    [ApiBad]
    [SwaggerOperation(OperationId = "AdrFolderUpdate")]
    public async Task<IActionResult> Update(UpdateAdrFolderApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<UpdateAdrFolderModel>(request);
        model.UserId = identityProvider.Id;
        await adrValidateService.ValidateAsync(model, cancellationToken);
        var result = await adrFolderManager.UpdateAsync(model, cancellationToken);
        return Ok(mapper.Map<AdrFolderApiModel>(result));
    }

    /// <summary>
    /// Удаляет существующую папку ADR вместе с вложенными папками и ADR
    /// </summary>
    [HttpDelete("{folderId:guid}")]
    [ApiNoContent]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [SwaggerOperation(OperationId = "AdrFolderDelete")]
    public async Task<IActionResult> Delete(Guid folderId, CancellationToken cancellationToken)
    {
        var model = new DeleteAdrFolderModel
        {
            UserId = identityProvider.Id,
            AdrFolderId = folderId,
        };
        await adrFolderManager.DeleteAsync(model, cancellationToken);
        return NoContent();
    }
}
