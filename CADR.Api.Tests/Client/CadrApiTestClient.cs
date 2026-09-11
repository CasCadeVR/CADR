using System.Net.Http.Headers;
using System.Text;
using CADR.Api.Client;

namespace CADR.Api.Tests.Client;

/// <inheritdoc />
internal class CadrApiTestClient : CadrApiClient, ICadrApiTestClient
{
    private readonly HttpClient httpClient;
    private readonly IBearerTokenProvider? bearerTokenProvider;

    private HttpResponseMessage? lastResponse;

    private const string SetCookieCommandName = "Set-Cookie";
    private const string CookieHeaderName = "Cookie";

    public CadrApiTestClient(string baseUrl, HttpClient httpClient, IBearerTokenProvider? bearerTokenProvider)
        : base(baseUrl, httpClient)
    {
        this.httpClient = httpClient;
        this.bearerTokenProvider = bearerTokenProvider;
    }

    protected override Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, string url, CancellationToken cancellationToken)
        => Task.CompletedTask;

    protected override Task PrepareRequestAsync(HttpClient client,
        HttpRequestMessage request,
        StringBuilder urlBuilder,
        CancellationToken cancellationToken)
    {
        if (bearerTokenProvider?.HasToken == true)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerTokenProvider.Token);
        }
        return Task.CompletedTask;
    }

    protected override Task ProcessResponseAsync(HttpClient client, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        lastResponse = response;
        return Task.CompletedTask;
    }

    IEnumerable<string> ICadrApiTestClient.GetCookieHeadersFromLastResponse()
        => lastResponse?.Headers.GetValues(SetCookieCommandName) ?? [];

    void ICadrApiTestClient.SetCookies(string cookies)
    {
        httpClient.DefaultRequestHeaders.Add(CookieHeaderName, cookies);
    }
}
