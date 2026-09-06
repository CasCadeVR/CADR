using AutoMapper;
using CADR.Administrations.Api.Models.Organizations;
using CADR.Administrations.Api.Resources;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Administrations.Services.Contracts.Models.Organizations;
using CADR.Common.Core.Contracts;
using CADR.Common.Mvc.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CADR.Administrations.Api.Controllers;

/// <summary>
/// Работа с организациями
/// </summary>
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = $"{AdministrationConstants.DocPrefix}v1")]
[Route(AdministrationConstants.DefaultControllerRoute)]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationManager organizationManager;
    private readonly IAdministrationValidateService administrationValidateService;
    private readonly IIdentityProvider identityProvider;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationController"/>
    /// </summary>
    public OrganizationController(IOrganizationManager organizationManager,
        IAdministrationValidateService administrationValidateService,
        IIdentityProvider identityProvider,
        IMapper mapper)
    {
        this.organizationManager = organizationManager;
        this.administrationValidateService = administrationValidateService;
        this.identityProvider = identityProvider;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получает список организаций
    /// </summary>
    [HttpGet]
    [ApiOk(typeof(IEnumerable<OrganizationApiModel>))]
    [ApiUnauthorized]
    [SwaggerOperation(OperationId = "OrganizationGet")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await organizationManager.GetByUserIdAsync(identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<IEnumerable<OrganizationApiModel>>(result));
    }

    /// <summary>
    /// Получает организацию по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ApiOk(typeof(OrganizationApiModel))]
    [ApiUnauthorized]
    [ApiNotFound]
    [SwaggerOperation(OperationId = "OrganizationGetById")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await organizationManager.GetByIdAsync(id, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<OrganizationApiModel>(result));
    }

    /// <summary>
    /// Создаёт новую организацию
    /// </summary>
    [HttpPost]
    [ApiOk(typeof(OrganizationApiModel))]
    [ApiConflict]
    [ApiUnauthorized]
    [SwaggerOperation(OperationId = "OrganizationCreate")]
    public async Task<IActionResult> Create(CreateOrganizationApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<CreateOrganizationModel>(request);
        await administrationValidateService.ValidateAsync(model, cancellationToken);
        model.UserId = identityProvider.Id;
        var result = await organizationManager.CreateAsync(model, cancellationToken);
        return Ok(mapper.Map<OrganizationApiModel>(result));
    }

    /// <summary>
    /// Обновляет организацию
    /// </summary>
    [HttpPut]
    [ApiOk(typeof(OrganizationApiModel))]
    [ApiNotFound]
    [ApiForbidden]
    [ApiValidation]
    [ApiUnauthorized]
    [SwaggerOperation(OperationId = "OrganizationUpdate")]
    public async Task<IActionResult> Update(OrganizationApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<UpdateOrganizationModel>(request);
        model.UserId = identityProvider.Id;
        await administrationValidateService.ValidateAsync(model, cancellationToken);
        var result = await organizationManager.UpdateAsync(model, cancellationToken);
        return Ok(mapper.Map<OrganizationApiModel>(result));
    }

    /// <summary>
    /// Удаляет организацию
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ApiNoContent]
    [ApiForbidden]
    [ApiValidation]
    [ApiNotFound]
    [ApiUnauthorized]
    [SwaggerOperation(OperationId = "OrganizationDelete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var model = new DeleteOrganizationModel
        {
            OrganizationId = id,
            UserId = identityProvider.Id
        };
        await organizationManager.DeleteAsync(model, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Получает пользователей организации
    /// </summary>
    [HttpGet("{id:guid}/users")]
    [ApiOk(typeof(IEnumerable<UserOrganizationApiModel>))]
    [ApiUnauthorized]
    [ApiNotFound]
    [SwaggerOperation(OperationId = "OrganizationUsers")]
    public async Task<IActionResult> GetUsers(Guid id, CancellationToken cancellationToken)
    {
        var result = await organizationManager.GetUsersByOrganizationIdAsync(id, identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<IEnumerable<UserOrganizationApiModel>>(result));
    }


    /// <summary>
    /// Удаляет пользователя из организации
    /// </summary>
    [HttpDelete("{organizationId:guid}/user/{userId:guid}")]
    [ApiNoContent]
    [ApiForbidden]
    [ApiNotFound]
    [ApiUnauthorized]
    [SwaggerOperation(OperationId = "OrganizationDeleteUser")]
    public async Task<IActionResult> DeleteUser(Guid userId, Guid organizationId, CancellationToken cancellationToken)
    {
        var model = new DeleteUserOrganizationModel
        {
            OrganizationId = organizationId,
            UserId = identityProvider.Id,
            UserToDeleteId = userId
        };

        await organizationManager.DeleteUserAsync(model, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Изменяет роль пользователя в организации
    /// </summary>
    [HttpPatch("{organizationId:guid}/users/{userToUpdateId:guid}")]
    [ApiNoContent]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [ApiNotAcceptable]
    [SwaggerOperation(OperationId = "OrganizationChangeUserRole")]
    public async Task<IActionResult> ChangeUserRole(ChangeUserRoleApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<ChangeUserRoleModel>(request);
        model.UserId = identityProvider.Id;

        await organizationManager.ChangeUserRoleAsync(model, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Получает текущего пользователя
    /// </summary>
    [HttpGet("{organizationId:guid}/current-user")]
    [ApiOk(typeof(UserOrganizationApiModel))]
    [ApiUnauthorized]
    [ApiForbidden]
    [ApiNotFound]
    [SwaggerOperation(OperationId = "OrganizationCurrentUser")]
    public async Task<IActionResult> GetCurrentUser(Guid organizationId, CancellationToken cancellationToken)
    {
        var result = await organizationManager.GetUserOrganizationAsync(identityProvider.Id, organizationId, cancellationToken);
        return Ok(mapper.Map<UserOrganizationApiModel>(result));
    }
}
