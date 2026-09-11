using System.Security.Claims;
using CADR.Api.Client;
using CADR.Portal.Contracts.Constants;
using CADR.Portal.Contracts.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CADR.Portal.Infrastructures
{
    /// <summary>
    /// Поставщик личности Cadr
    /// </summary>
    public class PortalIdentityProvider : IPortalIdentity
    {
        private readonly IPortalCadrApiClient client;
        private ClaimsPrincipal? principal;
        private bool needsToCheck = true;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="PortalIdentityProvider"/>
        /// </summary>
        public PortalIdentityProvider(IPortalCadrApiClient client)
        {
            this.client = client;
        }

        async Task IPortalIdentity.TryAuthenticate(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (principal != null || !needsToCheck)
            {
                return;
            }

            try
            {
                await FetchCurrentUser(cancellationToken);
            }
            catch (ApiException ex) when (ex.StatusCode == 401)
            {
                try
                {
                    await client.AccountRefreshCookieAsync(cancellationToken);
                    await FetchCurrentUser(cancellationToken);
                }
                catch
                {
                    principal = null;
                }
            }
            catch
            {
                principal = null;
            }

            needsToCheck = false;
            OnChangedAction?.Invoke();
        }

        void IPortalIdentity.InvalidateCache()
        {
            needsToCheck = principal == null;
            principal = null;
            OnChangedAction?.Invoke();
        }

        private async Task FetchCurrentUser(CancellationToken cancellationToken)
        {
            var userResult = await client.AccountGetCurrentUserAsync(cancellationToken);
            var claims = userResult.Claims.Select(c => new Claim(c.Key, c.Value));

            principal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    claims,
                    AuthenticationSchemesConstants.JwtCookie,
                    JwtRegisteredClaimNames.Name,
                    "role"
                )
            );
        }

        bool IPortalIdentity.IsAuthenticated => principal?.Identity?.IsAuthenticated == true;

        string IPortalIdentity.Name => GetClaimValueByType(JwtRegisteredClaimNames.Name);

        string IPortalIdentity.Login => GetClaimValueByType(JwtRegisteredClaimNames.GivenName);

        string IPortalIdentity.Email => GetClaimValueByType(JwtRegisteredClaimNames.Email);

        Guid IPortalIdentity.UserId
        {
            get
            {
                var claimUserId = GetClaimValueByType(JwtRegisteredClaimNames.NameId);
                return string.IsNullOrEmpty(claimUserId)
                    ? Guid.Empty
                    : Guid.Parse(claimUserId);
            }
        }

        bool IPortalIdentity.IsInRole(string roleName)
            => principal?.IsInRole(roleName) == true;

        IEnumerable<Claim> IPortalIdentity.Claims => GetClaims();

        /// <inheritdoc cref="IPortalIdentity.OnChangedAction"/>
        public Action? OnChangedAction { get; set; }

        private IEnumerable<Claim> GetClaims()
            => principal?.Claims ?? [];

        private string GetClaimValueByType(string claimType)
            => GetClaims().FirstOrDefault(x => x.Type == claimType)?.Value ?? string.Empty;
    }
}
