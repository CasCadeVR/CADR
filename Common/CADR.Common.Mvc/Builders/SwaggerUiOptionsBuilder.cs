using Asp.Versioning.ApiExplorer;
using CADR.Common.Mvc.Constants;
using CADR.Common.Mvc.Extensions;
using CADR.Common.Mvc.Models;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CADR.Common.Mvc.Builders;

/// <summary>
/// Построитель конечных точек для Swagger JSON
/// </summary>
public class SwaggerUiOptionsBuilder
{
    private readonly Action<SwaggerUiBuilderConfiguration> configurationAction;
    private readonly SwaggerUIOptions options;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SwaggerUiOptionsBuilder"/>
    /// </summary>
    private SwaggerUiOptionsBuilder(SwaggerUIOptions options,
        Action<SwaggerUiBuilderConfiguration> configurationAction)
    {
        this.options = options;
        this.configurationAction = configurationAction;
    }

    /// <summary>
    /// Создаёт <see cref="SwaggerUiOptionsBuilder"/> с указанием конфигурации
    /// </summary>
    public static SwaggerUiOptionsBuilder Create(SwaggerUIOptions options,
        Action<SwaggerUiBuilderConfiguration> configurationAction)
        => new(options, configurationAction);

    /// <summary>
    /// Строит конечные точки для Swagger JSON
    /// </summary>
    public void Build()
    {
        var config = new SwaggerUiBuilderConfiguration();
        configurationAction?.Invoke(config);
        var controllerGroupNames = config.TargetAssembly.GetApiExplorerSettingsGroupName();
        foreach (var description in config.ApiVersionDescriptionProvider?.ApiVersionDescriptions.Reverse() ?? Array.Empty<ApiVersionDescription>())
        {
            var groupName = description.GroupName;
            if (groupName == CommonMvcConsts.DefaultGroupName || controllerGroupNames.Contains(groupName))
            {
                options.SwaggerEndpoint($"swagger/{groupName}/swagger.json",
                    $"{config.DocName}: {description.GroupName.ToUpperInvariant()}");
            }
        }
    }
}
