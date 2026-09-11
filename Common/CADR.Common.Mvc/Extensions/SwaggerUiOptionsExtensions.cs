using CADR.Common.Mvc.Builders;
using CADR.Common.Mvc.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CADR.Common.Mvc.Extensions;

/// <summary>
/// Методы расширения для <see cref="SwaggerUIOptions"/>
/// </summary>
public static class SwaggerUiOptionsExtensions
{
    /// <summary>
    /// Добавляет конечную точку Swagger JSON
    /// </summary>
    public static SwaggerUiOptionsBuilder BuildSwaggerEndpoint(this SwaggerUIOptions options,
        SwaggerUiBuilderConfiguration configuration)
        => SwaggerUiOptionsBuilder.Create(options, x =>
        {
            x.ApiVersionDescriptionProvider = configuration.ApiVersionDescriptionProvider;
            x.DocPrefix = configuration.DocPrefix;
            x.DocName = configuration.DocName;
            x.TargetAssembly = configuration.TargetAssembly;
        });
}
