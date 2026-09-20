using Asp.Versioning.ApiExplorer;
using CADR.Administrations.Api.Infrastructures;
using CADR.Adrs.Api.Infrastructures;

namespace CADR.Api.Infrastructures;

static internal class DocumentationExtensions
{
    /// <summary>
    /// Регистрирует мидлварю для работы с документацией
    /// </summary>
    public static void UseDocumentation(this IApplicationBuilder applicationBuilder,
        IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        applicationBuilder.UseSwagger();
        applicationBuilder.UseSwaggerUI(options =>
        {
            options.SwaggerEndpointAdministrations(apiVersionDescriptionProvider);
            options.SwaggerEndpointAdrs(apiVersionDescriptionProvider);
            options.RoutePrefix = string.Empty;
            options.EnablePersistAuthorization();
        });
    }
}
