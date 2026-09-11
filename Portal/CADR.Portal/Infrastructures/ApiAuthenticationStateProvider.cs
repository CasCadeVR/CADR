using System.Security.Claims;
using CADR.Portal.Contracts.Constants;
using CADR.Portal.Contracts.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CADR.Portal.Infrastructures
{
    /// <summary>
    /// Переопределённый <see cref="AuthenticationStateProvider"/>
    /// </summary>
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IPortalIdentity portalIdentity;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ApiAuthenticationStateProvider"/>
        /// </summary>
        public ApiAuthenticationStateProvider(IPortalIdentity portalIdentity)
        {
            this.portalIdentity = portalIdentity;
            portalIdentity.OnChangedAction = StateChanged;
        }

        /// <inheritdoc cref="AuthenticationStateProvider.GetAuthenticationStateAsync"/>
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity());
            if (portalIdentity.IsAuthenticated)
            {
                principal = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        portalIdentity.Claims,
                        AuthenticationSchemesConstants.JwtCookie,
                        JwtRegisteredClaimNames.Name,
                        "role"
                    )
                );
            }
            return Task.FromResult(new AuthenticationState(principal));
        }

        private void StateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
