using CADR.Api.Client;
using CADR.Portal.Contracts.Interfaces;

namespace CADR.Portal.Infrastructures
{
    /// <inheritdoc cref="IRecentOrganizationsProvider"/>
    public class RecentOrganizationsProvider : IRecentOrganizationsProvider
    {
        private readonly IPortalCadrApiClient cadrApiClient;
        private readonly IPortalIdentity portalIdentity;
        private List<OrganizationApiModel>? cachedRecentOrganizations;
        private bool isActual;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="RecentOrganizationsProvider"/>
        /// </summary>
        public RecentOrganizationsProvider(IPortalCadrApiClient cadrApiClient,
            IPortalIdentity portalIdentity)
        {
            this.cadrApiClient = cadrApiClient;
            this.portalIdentity = portalIdentity;
            portalIdentity.OnChangedAction += HandleAuthenticationChanged;
        }

        void IRecentOrganizationsProvider.CheckOrganizations(Guid organizationId)
        {
            if (!isActual)
            {
                return;
            }
            isActual = cachedRecentOrganizations?.FirstOrDefault()?.Id == organizationId;
        }

        async Task<List<OrganizationApiModel>?> IRecentOrganizationsProvider.GetOrganizationsAsync(CancellationToken token)
        {
            if (!portalIdentity.IsAuthenticated)
            {
                return null;
            }

            if (cachedRecentOrganizations == null || !isActual)
            {
                cachedRecentOrganizations = (await cadrApiClient.OrganizationGetAsync(token)).ToList();
                isActual = true;
            }

            return cachedRecentOrganizations;
        }

        private void HandleAuthenticationChanged()
            => isActual = false;
    }
}
