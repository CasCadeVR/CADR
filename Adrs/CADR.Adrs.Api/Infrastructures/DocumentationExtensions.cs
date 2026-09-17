using System.Reflection;
using Asp.Versioning.ApiExplorer;
using CADR.Adrs.Api.Resources;
using CADR.Common.Mvc.Extensions;
using CADR.Common.Mvc.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CADR.Adrs.Api.Infrastructures;

/// <summary>
/// Расширение документации для сваггера
/// </summary>
public static class DocumentationExtensions
{
    /// <summary>
    /// Определяет один или несколько документов, которые будут созданы
    /// генератором Swagger для работы с ADR
    /// </summary>
    public static void SwaggerDocAccount(this SwaggerGenOptions swaggerGenOptions,
        IApiVersionDescriptionProvider provider)
        => swaggerGenOptions.BuildSwaggerDoc(GetBuilderConfiguration(provider)).Build();

    /// <summary>
    /// Добавляет swagger json endpoint для работы с ADR
    /// </summary>
    public static void SwaggerEndpointAccount(this SwaggerUIOptions options,
        IApiVersionDescriptionProvider provider)
        => options.BuildSwaggerEndpoint(GetBuilderConfiguration(provider)).Build();

    private static SwaggerBuilderConfiguration GetBuilderConfiguration(IApiVersionDescriptionProvider provider)
        => new()
        {
            ApiVersionDescriptionProvider = provider,
            TargetAssembly = Assembly.GetAssembly(typeof(AccountController)),
            DocName = AdrsConstants.DocName,
            DocPrefix = AdrsConstants.DocPrefix,
            Description = "API по работе с ADR",
        };
}
