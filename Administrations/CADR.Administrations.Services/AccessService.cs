using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Services.Contracts.Exceptions;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Common.Core.Contracts;

namespace CADR.Administrations.Services;

internal class AccessService : IAccessService, IAdministrationsServiceAnchor
{
    private readonly IIdentityProvider identityProvider;
    private readonly IOrganizationReadRepository organizationReadRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AccessService"/>
    /// </summary>
    public AccessService(IIdentityProvider identityProvider,
        IOrganizationReadRepository organizationReadRepository)
    {
        this.identityProvider = identityProvider;
        this.organizationReadRepository = organizationReadRepository;
    }

    async Task IAccessService.IsAdminForAnyUserOrganizationAsync(Guid userId, CancellationToken cancellationToken)
    {
        var currentUserId = identityProvider.Id;
        var isAdmin = await organizationReadRepository.IsUserAdminForOtherUserOrganizationAsync(currentUserId, userId, cancellationToken);
        if (!isAdmin)
        {
            throw new AdministrationAccessException();
        }
    }
}
