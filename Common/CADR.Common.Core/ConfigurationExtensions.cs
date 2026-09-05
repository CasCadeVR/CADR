using CADR.Common.Mvc.Models;
using Microsoft.Extensions.Configuration;

namespace CADR.Common.Core;

/// <summary>
/// Методы расширения для <see cref="IConfiguration"/>
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Получает конфигурацию <see cref="JwtSettingsModel"/>
    /// </summary>
    public static JwtSettingsModel GetJwtSettingsConfiguration(this IConfiguration configuration)
        => configuration.GetSection(JwtSettingsModel.Key).Get<JwtSettingsModel>()!;
}
