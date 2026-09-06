using AutoMapper;
using CADR.Administrations.Api.Models.Invites;
using CADR.Administrations.Api.Resources;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Administrations.Services.Contracts.Models.Invites;
using CADR.Common.Core.Contracts;
using CADR.Common.Mvc.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CADR.Administrations.Api.Controllers;

/// <summary>
/// Работа с приглашениями в организации
/// </summary>
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = $"{AdministrationConstants.DocPrefix}v1")]
[Route(AdministrationConstants.DefaultControllerRoute)]
public class InvitationsController : ControllerBase
{
    private readonly IIdentityProvider identityProvider;
    private readonly IMapper mapper;
    private readonly IAdministrationValidateService administrationValidateService;
    private readonly IInviteManager inviteManager;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="InvitationsController"/>
    /// </summary>
    public InvitationsController(IIdentityProvider identityProvider,
        IAdministrationValidateService administrationValidateService,
        IInviteManager inviteManager,
        IMapper mapper)
    {
        this.identityProvider = identityProvider;
        this.mapper = mapper;
        this.administrationValidateService = administrationValidateService;
        this.inviteManager = inviteManager;
    }

    /// <summary>
    /// Добавляет приглашение пользователя к организации
    /// </summary>
    [HttpPost("{organizationId:guid}")]
    [ApiNoContent]
    [ApiUnauthorized]
    [ApiValidation]
    [ApiNotFound]
    [ApiNotAcceptable]
    [SwaggerOperation(OperationId = "InviteAdd")]
    public async Task<IActionResult> AddInvite([FromRoute] Guid organizationId, [FromBody] InviteApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<InviteModel>(request);
        model.OrganizationId = organizationId;
        model.OwnerId = identityProvider.Id;
        await administrationValidateService.ValidateAsync(model, cancellationToken);
        await inviteManager.CreateInviteAsync(model, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Получает приглашения к организации
    /// </summary>
    [HttpGet("organization/{organizationId:guid}")]
    [ApiOk(typeof(IReadOnlyCollection<InviteOrganizationApiModel>))]
    [ApiUnauthorized]
    [ApiNotFound]
    [SwaggerOperation(OperationId = "InviteGetForOrganization")]
    public async Task<IActionResult> Invites([FromRoute] Guid organizationId, CancellationToken cancellationToken)
    {
        var result = await inviteManager.GetInvitesByOrganizationIdAsync(organizationId, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<IEnumerable<InviteOrganizationApiModel>>(result));
    }

    /// <summary>
    /// Удаляет приглашение к организации
    /// </summary>
    [HttpDelete("{inviteId:guid}")]
    [ApiNoContent]
    [ApiForbidden]
    [ApiNotFound]
    [ApiUnauthorized]
    [SwaggerOperation(OperationId = "InviteDelete")]
    public async Task<IActionResult> DeleteInvite(Guid inviteId, CancellationToken cancellationToken)
    {
        var model = new DeleteOrganizationInviteModel
        {
            UserId = identityProvider.Id,
            InviteId = inviteId,
        };
        await inviteManager.DeleteInviteAsync(model, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Принимает приглашение в организацию
    /// </summary>
    [HttpPut("accept/{inviteId:guid}")]
    [ApiNoContent]
    [ApiUnauthorized]
    [ApiNotFound]
    [ApiNotAcceptable]
    [SwaggerOperation(OperationId = "InviteAccept")]
    public async Task<IActionResult> AcceptInvite([FromRoute] Guid inviteId, CancellationToken cancellationToken)
    {
        await inviteManager.AcceptInviteAsync(inviteId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Получает приглашения к организациям, адресованные пользователю
    /// </summary>
    [HttpGet("user")]
    [ApiOk(typeof(IEnumerable<InviteForUserApiResponse>))]
    [ApiUnauthorized]
    [SwaggerOperation(OperationId = "InviteGetByUserId")]
    public async Task<IActionResult> GetInvitesByUserId(CancellationToken cancellationToken)
    {
        var invites = await inviteManager.GetInvitesByUserIdAsync(identityProvider.Id, cancellationToken);
        var result = mapper.Map<IEnumerable<InviteForUserApiResponse>>(invites);
        return Ok(result);
    }

    /// <summary>
    /// Отклоняет приглашение в организацию
    /// </summary>
    [HttpPut("reject/{inviteId:guid}")]
    [ApiNoContent]
    [ApiUnauthorized]
    [ApiForbidden]
    [SwaggerOperation(OperationId = "InviteReject")]
    public async Task<IActionResult> RejectInvite([FromRoute] Guid inviteId, CancellationToken cancellationToken)
    {
        await inviteManager.RejectInviteAsync(inviteId, identityProvider.Id, cancellationToken);
        return NoContent();
    }
}
