using AutoMapper;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Services.Contracts.Exceptions;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Administrations.Services.Contracts.Models.Enums;
using CADR.Administrations.Services.Contracts.Models.Organizations;
using CADR.Common.Core.Extensions;

namespace CADR.Administrations.Services.Organizations;

/// <inheritdoc cref="IOrganizationManager"/>
internal sealed class OrganizationManager : IOrganizationManager, IAdministrationsServiceAnchor
{
    private readonly IOrganizationReadRepository organizationReadRepository;
    private readonly IOrganizationWriteRepository organizationWriteRepository;
    private readonly IUserOrganizationReadRepository userOrganizationReadRepository;
    private readonly IUserOrganizationWriteRepository userOrganizationWriteRepository;
    private readonly IUserReadRepository userReadRepository;
    private readonly IAdministrationUnitOfWork unitOfWork;
    private readonly IUserInviteReadRepository userInviteReadRepository;
    private readonly IUserInviteWriteRepository userInviteWriteRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationManager"/>
    /// </summary>
    public OrganizationManager(IAdministrationUnitOfWork administrationUnitOfWork, IMapper mapper)
    {
        organizationReadRepository = administrationUnitOfWork.OrganizationReadRepository;
        organizationWriteRepository = administrationUnitOfWork.OrganizationWriteRepository;
        userOrganizationReadRepository = administrationUnitOfWork.UserOrganizationReadRepository;
        userOrganizationWriteRepository = administrationUnitOfWork.UserOrganizationWriteRepository;
        userReadRepository = administrationUnitOfWork.UserReadRepository;
        userInviteReadRepository = administrationUnitOfWork.UserInviteReadRepository;
        userInviteWriteRepository = administrationUnitOfWork.UserInviteWriteRepository;
        unitOfWork = administrationUnitOfWork;
        this.mapper = mapper;
    }

    async Task<OrganizationModel> IOrganizationManager.CreateAsync(CreateOrganizationModel model, CancellationToken cancellationToken)
    {
        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            NameLowerCase = model.Name.ToLower(),
            Description = model.Description,
        };
        organizationWriteRepository.Add(organization);
        var userOrganization = new UserOrganization
        {
            Id = Guid.NewGuid(),
            OrganizationId = organization.Id,
            UserId = model.UserId,
            Role = Role.Admin,
        };
        userOrganizationWriteRepository.Add(userOrganization);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<OrganizationModel>(organization);
    }

    async Task<IEnumerable<OrganizationModel>> IOrganizationManager.GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var items = await organizationReadRepository.GetByUserIdAsync(userId, cancellationToken);
        var userOrganizationRoles =
            await userOrganizationReadRepository.GetRoleByUserAndOrganizationIdsAsync(userId, items.Select(x => x.Id).ToReadOnlyCollection(),
                cancellationToken);
        var result = mapper.Map<IEnumerable<OrganizationModel>>(items).ToReadOnlyCollection();
        foreach (var organizationModel in result)
        {
            organizationModel.UserIsAdmin = userOrganizationRoles[organizationModel.Id] == Role.Admin;
        }

        return result;
    }

    async Task<OrganizationModel> IOrganizationManager.GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var item = await organizationReadRepository.GetActiveByIdAsync(id, userId, cancellationToken)
            .OrThrowIfNull(() => new AdministrationEntityNotFoundException<Organization>(id));
        var userOrganization = await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(userId, id, cancellationToken);

        var result = mapper.Map<OrganizationModel>(item);
        result.UserIsAdmin = userOrganization!.Role == Role.Admin;

        return result;
    }

    async Task<OrganizationModel> IOrganizationManager.UpdateAsync(UpdateOrganizationModel model, CancellationToken cancellationToken)
    {
        var organization = await GetOrganization(model.Id, model.UserId, cancellationToken);
        await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(model.UserId,
                model.Id,
                cancellationToken)
            .OrThrowIf(x => x?.Role != Role.Admin, () => new AdministrationAccessException());

        organization.Name = model.Name;
        organization.NameLowerCase = model.Name.ToLower();
        organization.Description = model.Description;

        organizationWriteRepository.Update(organization);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<OrganizationModel>(organization);
    }

    async Task IOrganizationManager.DeleteAsync(DeleteOrganizationModel model, CancellationToken cancellationToken)
    {
        var organization = await GetOrganization(model.OrganizationId, model.UserId, cancellationToken);
        await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(model.UserId,
                model.OrganizationId,
                cancellationToken)
            .OrThrowIf(x => x?.Role != Role.Admin, () => new AdministrationAccessException());

        organizationWriteRepository.Delete(organization);

        var userOrganizations = await userOrganizationReadRepository.GetByOrganizationIdAsync(model.OrganizationId, cancellationToken);
        foreach (var item in userOrganizations)
        {
            userOrganizationWriteRepository.Delete(item);
        }

        var invites = await userInviteReadRepository.GetActualByOrganizationIdAsync(model.OrganizationId, cancellationToken);
        foreach (var item in invites)
        {
            userInviteWriteRepository.Delete(item);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    async Task<IEnumerable<UserOrganizationModel>> IOrganizationManager.GetUsersByOrganizationIdAsync(Guid id,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var organization = await GetOrganization(id, userId, cancellationToken);
        var userOrganizations = await userOrganizationReadRepository.GetByOrganizationIdAsync(organization.Id, cancellationToken);
        var users = await userReadRepository.GetByOrganizationIdAsync(organization.Id, cancellationToken);
        var result = (from uo in userOrganizations
                      join u in users on uo.UserId equals u.Id
                      select new UserOrganizationModel
                      {
                          Id = u.Id,
                          Name = u.Name,
                          Email = u.Email,
                          Login = u.Login,
                          Blocked = u.Blocked,
                          BlockedAt = u.BlockedAt,
                          Role = mapper.Map<UserRole>(uo.Role),
                      }
            )
            .OrderBy(x => x.Name.ToLower())
            .ToList();
        return result;
    }

    async Task IOrganizationManager.ChangeUserRoleAsync(ChangeUserRoleModel model, CancellationToken cancellationToken)
    {
        if (model.UserId == model.UserToUpdateId)
        {
            throw new AdministrationInvalidOperationException("Вы не можете изменить свою роль");
        }

        await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(model.UserId, model.OrganizationId, cancellationToken)
            .OrThrowIf(x => x?.Role != Role.Admin, () => new AdministrationAccessException());

        var targetUser = await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(model.UserToUpdateId, model.OrganizationId, cancellationToken)
            .OrThrowIfNull(() => new AdministrationEntityNotFoundException<User>(model.UserToUpdateId));
        var roleToSet = mapper.Map<Role>(model.Role);

        if (targetUser!.Role == roleToSet)
        {
            throw new AdministrationInvalidOperationException("У этого пользователя уже установлена такая роль");
        }

        targetUser.Role = mapper.Map<Role>(model.Role);
        userOrganizationWriteRepository.Update(targetUser);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    async Task IOrganizationManager.DeleteUserAsync(DeleteUserOrganizationModel model, CancellationToken cancellationToken)
    {
        await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(model.UserId,
                model.OrganizationId,
                cancellationToken)
            .OrThrowIf(x => x?.Role != Role.Admin, () => new AdministrationAccessException());

        var userOrganizationToDelete = await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(model.UserToDeleteId,
                model.OrganizationId,
                cancellationToken)
            .OrThrowIfNull(() => new AdministrationEntityNotFoundException<User>(model.UserToDeleteId));

        userOrganizationWriteRepository.Delete(userOrganizationToDelete!);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserOrganizationModel> GetUserOrganizationAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken)
    {
        var userOrganization = await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(userId, organizationId, cancellationToken)
            .OrThrowIfNull(() => new AdministrationAccessException());
        var user = await userReadRepository.GetByIdAsync(userOrganization!.UserId, cancellationToken)
            .OrThrowIfNull(() => new AdministrationEntityNotFoundException<User>(userOrganization.UserId));

        var result = MapUserOrganization(userOrganization, user!);

        return result;
    }

    private async Task<Organization> GetOrganization(Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        var organization = await organizationReadRepository.GetActiveByIdAsync(organizationId, userId, cancellationToken)
            .OrThrowIfNull(() => new AdministrationEntityNotFoundException<Organization>(organizationId));
        return organization!;
    }

    private UserOrganizationModel MapUserOrganization(UserOrganization userOrganization, User user)
        => new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Login = user.Login,
            Blocked = user.Blocked,
            BlockedAt = user.BlockedAt,
            Role = mapper.Map<UserRole>(userOrganization.Role),
        };
}
