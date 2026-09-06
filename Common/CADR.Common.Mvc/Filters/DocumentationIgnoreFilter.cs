using System.Reflection;
using CADR.Common.Core.Contracts.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CADR.Common.Mvc.Filters;

/// <summary>
/// Обрабатывает свойства, которые не нужно включать в OpenAPI
/// </summary>
public class DocumentationIgnoreFilter : IOperationFilter
{
    void IOperationFilter.Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Получаем типы параметров эндпоинта
        var parameterTypes = context.MethodInfo.GetParameters()
            .Select(p => p.ParameterType)
            .ToList();

        // Список имён свойств, помеченных атрибутом DocumentationIgnoreAttribute во всех типах параметров
        var excludedProperties = parameterTypes
            .SelectMany(t => t.GetProperties()
                .Where(prop => prop.GetCustomAttribute<DocumentationIgnoreAttribute>() != null)
                .Select(prop => prop.Name))
            .ToList();

        // Удаляем параметры из operation.Parameters, если их имя совпадает с исключённым свойством
        operation.Parameters = operation.Parameters
            .Where(p => !excludedProperties.Contains(p.Name))
            .ToList();
    }
}
