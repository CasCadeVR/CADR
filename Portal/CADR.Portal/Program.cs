using CADR.Portal;
using CADR.Portal.Infrastructures;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

if (builder.HostEnvironment.IsProduction())
{
    builder.Services.AddLogging(config =>
    {
        config.ClearProviders();
    });
}
builder.Services.AddHttpClient();
builder.Services.AddCadrApiClient(builder.Configuration);
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddIdentity();
builder.Services.AddJsEventHandlers();
builder.Services.AddDependencyInjection();

await builder.Build().RunAsync();
