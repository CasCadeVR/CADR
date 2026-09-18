using AutoMapper;
using CADR.Adrs.Api.Models.Adrs;
using CADR.Adrs.Api.Models.Enums;
using CADR.Adrs.Api.Resources;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Adrs;
using CADR.Adrs.Services.Contracts.Models.Enums;
using CADR.Common.Core.Contracts;
using CADR.Common.Mvc.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CADR.Adrs.Api.Controllers;

/// <summary>
/// Работа с ADR
/// </summary>
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = $"{AdrsConstants.DocPrefix}v1")]
[Route(AdrsConstants.DefaultControllerRoute)]
public class AdrController : ControllerBase
{
    private readonly IAdrManager adrManager;
    private readonly IAdrExportService adrExportService;
    private readonly IAdrValidateService adrValidateService;
    private readonly IIdentityProvider identityProvider;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrController"/>
    /// </summary>
    public AdrController(IAdrManager adrManager,
        IAdrExportService adrExportService,
        IAdrValidateService adrValidateService,
        IIdentityProvider identityProvider,
        IMapper mapper)
    {
        this.adrManager = adrManager;
        this.adrExportService = adrExportService;
        this.adrValidateService = adrValidateService;
        this.identityProvider = identityProvider;
        this.mapper = mapper;
    }

    /// <summary>
    /// Создаёт новый ADR
    /// </summary>
    [HttpPost]
    [ApiOk(typeof(AdrApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiValidation]
    [ApiBad]
    [SwaggerOperation(OperationId = "AdrCreate")]
    public async Task<IActionResult> Create(CreateAdrApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<CreateAdrModel>(request);
        await adrValidateService.ValidateAsync(model, cancellationToken);
        model.AuthorId = identityProvider.Id;
        var result = await adrManager.CreateAdrAsync(model, cancellationToken);
        return Ok(mapper.Map<AdrApiModel>(result));
    }

    /// <summary>
    /// Получает ADR по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ApiOk(typeof(AdrApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [SwaggerOperation(OperationId = "AdrGet")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await adrManager.GetByIdAsync(id, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<AdrApiModel>(result));
    }

    /// <summary>
    /// Обновляет существующий ADR вместе с разделами
    /// </summary>
    [HttpPut]
    [ApiOk(typeof(AdrApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [ApiValidation]
    [ApiBad]
    [SwaggerOperation(OperationId = "AdrUpdate")]
    public async Task<IActionResult> Update(UpdateAdrApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<UpdateAdrModel>(request);
        await adrValidateService.ValidateAsync(model, cancellationToken);
        model.UserId = identityProvider.Id;
        var result = await adrManager.UpdateAdrAsync(model, cancellationToken);
        return Ok(mapper.Map<AdrApiModel>(result));
    }

    /// <summary>
    /// Удаляет существующий ADR
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ApiNoContent]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [SwaggerOperation(OperationId = "AdrDelete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var model = new DeleteAdrModel
        {
            UserId = identityProvider.Id,
            AdrId = id,
        };
        await adrManager.DeleteAdrAsync(model, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Меняет статус ADR
    /// </summary>
    [HttpPatch("{id:guid}/status")]
    [ApiNoContent]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [ApiBad]
    [SwaggerOperation(OperationId = "AdrChangeStatus")]
    public async Task<IActionResult> ChangeStatus(Guid id, ChangeAdrStatusApiModel request, CancellationToken cancellationToken)
    {
        var model = new ChangeAdrStatusModel
        {
            UserId = identityProvider.Id,
            AdrId = id,
            Status = mapper.Map<AdrStatus>(request.Status),
        };
        await adrManager.ChangeStatusAsync(model, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Голосует за ADR или меняет существующий голос
    /// </summary>
    [HttpPost("{id:guid}/votes")]
    [ApiNoContent]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [ApiBad]
    [SwaggerOperation(OperationId = "AdrVote")]
    public async Task<IActionResult> Vote(Guid id, VoteAdrApiModel request, CancellationToken cancellationToken)
    {
        var model = new VoteAdrModel
        {
            UserId = identityProvider.Id,
            AdrId = id,
            Vote = mapper.Map<AdrVoteType>(request.Vote),
        };
        await adrManager.VoteAsync(model, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Снимает голос текущего пользователя с ADR
    /// </summary>
    [HttpDelete("{id:guid}/votes")]
    [ApiNoContent]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [ApiBad]
    [SwaggerOperation(OperationId = "AdrWithdrawVote")]
    public async Task<IActionResult> WithdrawVote(Guid id, CancellationToken cancellationToken)
    {
        var model = new WithdrawVoteAdrModel
        {
            UserId = identityProvider.Id,
            AdrId = id,
        };
        await adrManager.WithdrawVoteAsync(model, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Получает список всех ADR организации
    /// </summary>
    [HttpGet("organization/{organizationId:guid}")]
    [ApiOk(typeof(IEnumerable<AdrApiModel>))]
    [ApiUnauthorized]
    [ApiForbidden]
    [SwaggerOperation(OperationId = "AdrGetByOrganization")]
    public async Task<IActionResult> GetByOrganization(Guid organizationId, CancellationToken cancellationToken)
    {
        var result = await adrManager.GetByOrganizationIdAsync(organizationId, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<IEnumerable<AdrApiModel>>(result));
    }

    /// <summary>
    /// Получает список недавно добавленных или обновлённых ADR организации
    /// </summary>
    [HttpGet("organization/{organizationId:guid}/recent")]
    [ApiOk(typeof(IEnumerable<AdrApiModel>))]
    [ApiUnauthorized]
    [ApiForbidden]
    [SwaggerOperation(OperationId = "AdrGetRecent")]
    public async Task<IActionResult> GetRecent(Guid organizationId, [FromQuery] int count, CancellationToken cancellationToken)
    {
        var maxCount = count > 0 ? count : 10;
        var result = await adrManager.GetRecentAsync(organizationId, identityProvider.Id, maxCount, cancellationToken);
        return Ok(mapper.Map<IEnumerable<AdrApiModel>>(result));
    }

    /// <summary>
    /// Получает список ADR организации, отфильтрованных по статусам
    /// </summary>
    [HttpGet("organization/{organizationId:guid}/by-status")]
    [ApiOk(typeof(IEnumerable<AdrApiModel>))]
    [ApiUnauthorized]
    [ApiForbidden]
    [SwaggerOperation(OperationId = "AdrGetByStatuses")]
    public async Task<IActionResult> GetByStatuses(Guid organizationId, [FromQuery] ICollection<AdrStatusApi> statuses, CancellationToken cancellationToken)
    {
        var modelStatuses = statuses.Select(x => mapper.Map<AdrStatus>(x)).ToList();
        var result = await adrManager.GetByStatusesAsync(organizationId, modelStatuses, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<IEnumerable<AdrApiModel>>(result));
    }

    /// <summary>
    /// Получает список ADR папки организации (без указания папки - ADR в корне организации)
    /// </summary>
    [HttpGet("organization/{organizationId:guid}/folder/{folderId:guid?}")]
    [ApiOk(typeof(IEnumerable<AdrApiModel>))]
    [ApiUnauthorized]
    [ApiForbidden]
    [SwaggerOperation(OperationId = "AdrGetByFolder")]
    public async Task<IActionResult> GetByFolder(Guid organizationId, Guid? folderId, CancellationToken cancellationToken)
    {
        var result = await adrManager.GetByFolderIdAsync(organizationId, folderId, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<IEnumerable<AdrApiModel>>(result));
    }

    /// <summary>
    /// Получает список ADR указанного автора в организации
    /// </summary>
    [HttpGet("organization/{organizationId:guid}/user/{userId:guid}")]
    [ApiOk(typeof(IEnumerable<AdrApiModel>))]
    [ApiUnauthorized]
    [ApiForbidden]
    [SwaggerOperation(OperationId = "AdrGetByAuthor")]
    public async Task<IActionResult> GetByAuthor(Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        var result = await adrManager.GetByAuthorIdAsync(organizationId, userId, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<IEnumerable<AdrApiModel>>(result));
    }

    /// <summary>
    /// Экспортирует ADR в Markdown (MADR-подобный формат)
    /// </summary>
    [HttpGet("{id:guid}/export")]
    [ApiOk(typeof(string))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [SwaggerOperation(OperationId = "AdrExport")]
    public async Task<IActionResult> Export(Guid id, CancellationToken cancellationToken)
    {
        var result = await adrExportService.ExportToMarkdownAsync(id, identityProvider.Id, cancellationToken);
        return Content(result, "text/markdown");
    }

    /// <summary>
    /// Экспортирует все утверждённые ADR организации в общий Markdown-документ
    /// </summary>
    [HttpGet("organization/{organizationId:guid}/export")]
    [ApiOk(typeof(string))]
    [ApiUnauthorized]
    [ApiForbidden]
    [SwaggerOperation(OperationId = "AdrExportApproved")]
    public async Task<IActionResult> ExportApproved(Guid organizationId, CancellationToken cancellationToken)
    {
        var result = await adrExportService.ExportApprovedToMarkdownAsync(organizationId, identityProvider.Id, cancellationToken);
        return Content(result, "text/markdown");
    }
}
