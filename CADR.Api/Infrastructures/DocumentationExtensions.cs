using Asp.Versioning.ApiExplorer;
using CADR.Administrations.Api.Infrastructures;

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
            options.SwaggerEndpointAccount(apiVersionDescriptionProvider);
            options.RoutePrefix = string.Empty;
            options.EnablePersistAuthorization();
        });
    }
}
