using CADR.Api.Infrastructures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CADR.Api.Tests.Helpers;

static internal class WebHostBuilderHelper
{
    /// <summary>
    /// Конфигурирование IWebHostBuilder
    /// </summary>
    public static void ConfigureTestAppConfiguration(this IWebHostBuilder builder)
    {
        builder.UseEnvironment(ApiConstants.IntegrationgTestingEnvironment);
        builder.ConfigureAppConfiguration((_, config) =>
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.integration.json");
            config.AddJsonFile(configPath).AddEnvironmentVariables();
        });
    }
}
