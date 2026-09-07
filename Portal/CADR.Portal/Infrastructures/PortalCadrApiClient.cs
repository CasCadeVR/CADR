using System.Text;
using CADR.Api.Client;
using CADR.Portal.Contracts.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace CADR.Portal.Infrastructures
{
    /// <inheritdoc cref="IPortalCadrApiClient"/>
    internal class PortalCadrApiClient : CadrApiClient, IPortalCadrApiClient
    {
        private bool isAnonymousRequest;

        private readonly BrowserRequestCredentials credentials;
        private readonly Guid clientId;

        public PortalCadrApiClient(string baseUrl,
            BrowserRequestCredentials credentials,
            HttpClient httpClient)
            : base(baseUrl, httpClient)
        {
            clientId = Guid.NewGuid();
            this.credentials = credentials;
        }

        public IPortalCadrApiClient AllowAnonymous(bool isAnonymous)
        {
            isAnonymousRequest = isAnonymous;
            return this;
        }

        protected override Task PrepareRequestAsync(HttpClient client,
            HttpRequestMessage request,
            string url,
            CancellationToken cancellationToken)
        {
            request.SetBrowserRequestCredentials(credentials);
            return Task.CompletedTask;
        }

        protected override Task PrepareRequestAsync(HttpClient client,
            HttpRequestMessage request,
            StringBuilder urlBuilder,
            CancellationToken cancellationToken)
            => Task.CompletedTask;

        protected override Task ProcessResponseAsync(HttpClient client, HttpResponseMessage response, CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
