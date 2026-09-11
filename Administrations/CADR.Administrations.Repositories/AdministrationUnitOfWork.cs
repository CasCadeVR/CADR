using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace CADR.Administrations.Repositories;

/// <inheritdoc cref="IAdministrationUnitOfWork"/>
/// <remarks>
/// На реализацию UnitOfWork нужно дописать тест в CADR.Api.Tests.UnitOfWorkTests, проверяющий все свойства
/// </remarks>
internal class AdministrationUnitOfWork : IAdministrationUnitOfWork, IAdministrationRepositoryAnchor
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IServiceProvider serviceProvider;

    public AdministrationUnitOfWork(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        this.unitOfWork = unitOfWork;
        this.serviceProvider = serviceProvider;
    }

    Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
        => unitOfWork.SaveChangesAsync(cancellationToken);

    IOrganizationReadRepository IAdministrationUnitOfWork.OrganizationReadRepository
        => serviceProvider.GetRequiredService<IOrganizationReadRepository>();

    IOrganizationWriteRepository IAdministrationUnitOfWork.OrganizationWriteRepository
        => serviceProvider.GetRequiredService<IOrganizationWriteRepository>();

    IRefreshTokenReadRepository IAdministrationUnitOfWork.RefreshTokenReadRepository
        => serviceProvider.GetRequiredService<IRefreshTokenReadRepository>();

    IRefreshTokenWriteRepository IAdministrationUnitOfWork.RefreshTokenWriteRepository
        => serviceProvider.GetRequiredService<IRefreshTokenWriteRepository>();

    IUserOrganizationReadRepository IAdministrationUnitOfWork.UserOrganizationReadRepository
        => serviceProvider.GetRequiredService<IUserOrganizationReadRepository>();

    IUserOrganizationWriteRepository IAdministrationUnitOfWork.UserOrganizationWriteRepository
        => serviceProvider.GetRequiredService<IUserOrganizationWriteRepository>();

    IUserReadRepository IAdministrationUnitOfWork.UserReadRepository
        => serviceProvider.GetRequiredService<IUserReadRepository>();

    IUserWriteRepository IAdministrationUnitOfWork.UserWriteRepository
        => serviceProvider.GetRequiredService<IUserWriteRepository>();

    IUserInviteReadRepository IAdministrationUnitOfWork.UserInviteReadRepository
        => serviceProvider.GetRequiredService<IUserInviteReadRepository>();

    IUserInviteWriteRepository IAdministrationUnitOfWork.UserInviteWriteRepository
        => serviceProvider.GetRequiredService<IUserInviteWriteRepository>();
}
