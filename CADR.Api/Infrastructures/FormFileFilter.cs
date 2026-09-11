using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CADR.Api.Infrastructures;

/// <summary>
/// Фильтр для замены <see cref="IFormFile"/> на multipart/form-data для отображения
/// </summary>
internal class FormFileFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameters = context.ApiDescription.ActionDescriptor.Parameters;
        var hasFormFile = false;
        var properties = new Dictionary<string, OpenApiSchema>();
        var schema = new OpenApiSchema { Type = "string", Format = "binary" };
        foreach (var parameter in parameters)
        {
            if (parameter.ParameterType == typeof(IFormFile))
            {
                hasFormFile = true;
                properties.Add(parameter.Name, schema);
            }
        }

        if (hasFormFile)
        {
            var mediaType = new OpenApiMediaType { Schema = new OpenApiSchema { Type = "object", Properties = properties } };
            operation.RequestBody = new OpenApiRequestBody { Content = { ["multipart/form-data"] = mediaType } };
        }
    }
}
