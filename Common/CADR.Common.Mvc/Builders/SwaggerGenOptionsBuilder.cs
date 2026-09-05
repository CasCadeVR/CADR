using Asp.Versioning.ApiExplorer;
using CADR.Common.Mvc.Constants;
using CADR.Common.Mvc.Extensions;
using CADR.Common.Mvc.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CADR.Common.Mvc.Builders;

/// <summary>
/// Построитель документации для Swagger генератора
/// </summary>
public class SwaggerGenOptionsBuilder
{
    private readonly Action<SwaggerBuilderConfiguration> configurationAction;
    private readonly SwaggerGenOptions swaggerGenOptions;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SwaggerGenOptionsBuilder"/>
    /// </summary>
    private SwaggerGenOptionsBuilder(SwaggerGenOptions swaggerGenOptions,
        Action<SwaggerBuilderConfiguration> configurationAction)
    {
        this.swaggerGenOptions = swaggerGenOptions;
        this.configurationAction = configurationAction;
    }

    /// <summary>
    /// Создаёт <see cref="SwaggerGenOptionsBuilder"/> с указанием конфигурации
    /// </summary>
    public static SwaggerGenOptionsBuilder Create(SwaggerGenOptions swaggerGenOptions,
        Action<SwaggerBuilderConfiguration> configurationAction)
        => new(swaggerGenOptions, configurationAction);

    /// <summary>
    /// Строит документацию для Swagger генератора
    /// </summary>
    public void Build()
    {
        var config = new SwaggerBuilderConfiguration();
        configurationAction?.Invoke(config);
        var controllerGroupNames = config.TargetAssembly.GetApiExplorerSettingsGroupName();
        foreach (var description in config.ApiVersionDescriptionProvider?.ApiVersionDescriptions ?? Array.Empty<ApiVersionDescription>())
        {
            var groupName = description.GroupName;
            if (groupName == CommonMvcConsts.DefaultGroupName || controllerGroupNames.Contains(groupName))
            {
                swaggerGenOptions.SwaggerDoc(groupName, CreateVersionInfo(description, config));
            }
        }
    }

    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription desc,
        SwaggerBuilderConfiguration configuration)
    {
        var info = new OpenApiInfo { Title = configuration.DocName, Description = configuration.Description, Version = desc.ApiVersion.ToString() };

        if (desc.IsDeprecated)
        {
            info.Description += CommonMvcConsts.ApiVersionDeprecatedMessage;
        }

        return info;
    }
}
