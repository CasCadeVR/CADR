using AutoMapper;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Services.Contracts.Exceptions;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Administrations.Services.Contracts.Models.Enums;
using CADR.Administrations.Services.Contracts.Models.Invites;
using CADR.Administrations.Services.Helpers;
using CADR.Common.Core.Extensions;

namespace CADR.Administrations.Services.Invite;

/// <inheritdoc cref="IInviteManager"/>
internal sealed class InviteManager : IInviteManager, IAdministrationsServiceAnchor
{
    private readonly IMapper mapper;
    private readonly IAdministrationUnitOfWork unitOfWork;
    private readonly IUserReadRepository userReadRepository;
    private readonly IUserInviteWriteRepository userInviteWriteRepository;
    private readonly IUserInviteReadRepository userInviteReadRepository;
    private readonly IUserOrganizationReadRepository userOrganizationReadRepository;
    private readonly IUserOrganizationWriteRepository userOrganizationWriteRepository;
    private readonly IOrganizationReadRepository organizationReadRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="InviteManager"/>
    /// </summary>
    public InviteManager(IAdministrationUnitOfWork administrationUnitOfWork, IMapper mapper)
    {
        this.mapper = mapper;
        unitOfWork = administrationUnitOfWork;
        userReadRepository = administrationUnitOfWork.UserReadRepository;
        userInviteWriteRepository = administrationUnitOfWork.UserInviteWriteRepository;
        userInviteReadRepository = administrationUnitOfWork.UserInviteReadRepository;
        userOrganizationReadRepository = administrationUnitOfWork.UserOrganizationReadRepository;
        userOrganizationWriteRepository = administrationUnitOfWork.UserOrganizationWriteRepository;
        organizationReadRepository = administrationUnitOfWork.OrganizationReadRepository;
    }

    async Task IInviteManager.RejectInviteAsync(Guid inviteId, Guid userId, CancellationToken cancellationToken)
    {
        var invite = await userInviteReadRepository.GetActualByIdAsync(inviteId, cancellationToken);
        if (invite == null || userId != invite.UserId)
        {
            throw new AdministrationAccessException();
        }
        userInviteWriteRepository.Delete(invite);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    async Task IInviteManager.AcceptInviteAsync(Guid inviteId, CancellationToken cancellationToken)
    {
        var invite = await userInviteReadRepository.GetActualByIdAsync(inviteId, cancellationToken)
            .OrThrowIfNull(() => new AdministrationEntityNotFoundException<UserInvite>(inviteId));

        var userOfOrganization = await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(invite!.UserId, invite.OrganizationId, cancellationToken);
        if (userOfOrganization != null)
        {
            var organization = await organizationReadRepository.GetActiveByIdAsync(invite.OrganizationId, userOfOrganization.Id, cancellationToken);
            var user = await userReadRepository.GetByIdAsync(invite.UserId, cancellationToken);
            var message = $"Пользователь с адресом = {user?.Email} уже добавлен в организацию {organization?.Name}";
            throw new AdministrationInvalidOperationException(message);
        }
        var addedUserOrganization = new UserOrganization()
        {
            Id = Guid.NewGuid(),
            OrganizationId = invite.OrganizationId,
            UserId = invite.UserId,
            Role = invite.Role,
        };
        userOrganizationWriteRepository.Add(addedUserOrganization);
        userInviteWriteRepository.Delete(invite);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    async Task IInviteManager.CreateInviteAsync(InviteModel model, CancellationToken cancellationToken)
    {
        var ownUserOrganization = await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(model.OwnerId, model.OrganizationId, cancellationToken)
            .OrThrowIfNull(() => new AdministrationEntityNotFoundException<UserOrganization>(model.OrganizationId));
        var user = await userReadRepository.GetActiveByEmailAsync(model.UserMail, cancellationToken)
            .OrThrowIfNull(() => new AdministrationNotFoundException($"Пользователь с адресом '{model.UserMail}' не найден"));
        await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(user!.Id, model.OrganizationId, cancellationToken)
            .OrThrowIf(x => x != null, () => new AdministrationInvalidOperationException($"Пользователь с адресом '{model.UserMail}' уже добавлен в организацию"));

        await userInviteReadRepository.GetActualByUserAndOrganizationIdAsync(user.Id, model.OrganizationId, cancellationToken)
            .OrThrowIf(x => x != null,
            x => new AdministrationInvalidOperationException($"Пользователь с адресом '{model.UserMail}' уже получал приглашение {x!.CreatedAt:d.M.yyyy HH:mm}"));

        var userInvite = new UserInvite
        {
            Id = Guid.NewGuid(),
            OrganizationId = model.OrganizationId,
            UserId = user.Id,
            Role = RoleHelper.Min(ownUserOrganization!.Role, mapper.Map<Role>(model.Role)),
        };
        userInviteWriteRepository.Add(userInvite);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    async Task IInviteManager.DeleteInviteAsync(DeleteOrganizationInviteModel model, CancellationToken cancellationToken)
    {
        var item = await userInviteReadRepository.GetActualByIdAsync(model.InviteId, cancellationToken)
            .OrThrowIfNull(() => new AdministrationEntityNotFoundException<UserInvite>(model.InviteId));
        await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(model.UserId,
            item!.OrganizationId,
            cancellationToken)
        .OrThrowIf(x => x?.Role != Role.Admin, () => new AdministrationAccessException());

        userInviteWriteRepository.Delete(item!);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    async Task<IReadOnlyCollection<InviteOrganizationModel>> IInviteManager.GetInvitesByOrganizationIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        await organizationReadRepository.GetActiveByIdAsync(id, userId, cancellationToken)
            .OrThrowIfNull(() => new AdministrationEntityNotFoundException<Entities.Organization>(id));
        var invites = await userInviteReadRepository.GetActualWthUserByOrganizationIdAsync(id, cancellationToken);
        var result = invites.Select(x => new InviteOrganizationModel
        {
            Id = x.Id,
            UserName = x.User!.Name,
            UserMail = x.User!.Email,
            Role = mapper.Map<UserRole>(x.Role),
            CreatedAt = x.CreatedAt,
            CreatedBy = x.CreatedBy,
        })
            .OrderBy(x => x.UserMail)
            .ToReadOnlyCollection();

        return result;
    }

    async Task<IEnumerable<InviteForUserModel>> IInviteManager.GetInvitesByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var invites = await userInviteReadRepository.GetActualByUserIdAsync(userId, cancellationToken);
        var result = mapper.Map<IReadOnlyCollection<InviteForUserModel>>(invites);
        var organizationIds = result.Select(x => x.OrganizationId).Distinct().ToList();
        var organizations = await organizationReadRepository.GetActiveByCollectionIdAsync(organizationIds, cancellationToken);
        foreach (var invite in result)
        {
            var organization = organizations[invite.OrganizationId];
            invite.OrganizationDescription = organization.Description;
            invite.OrganizationName = organization.Name;
        }
        return result;
    }
}
