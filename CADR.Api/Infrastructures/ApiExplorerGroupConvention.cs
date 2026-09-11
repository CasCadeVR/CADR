using CADR.Common.Mvc.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CADR.Api.Infrastructures;

/// <summary>
/// Добавление стандартного значения <see cref="ApiExplorerSettingsAttribute"/> для всех контроллеров
/// </summary>
public class ApiExplorerGroupConvention : IControllerModelConvention
{
    /// <inheritdoc cref="IControllerModelConvention.Apply"/>
    public void Apply(ControllerModel controller)
    {
        if (string.IsNullOrEmpty(controller.ApiExplorer.GroupName))
        {
            controller.ApiExplorer.GroupName = CommonMvcConsts.DefaultGroupName;
        }
    }
}
