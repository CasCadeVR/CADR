using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace CADR.Portal.Components.Infrastructures;

/// <summary>
/// Компонент для вывода сообщения об ошибке валидации модели
/// </summary>
public class ValidationModelSummary : ComponentBase
{
    private string modelValidationMessage = string.Empty;

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created <c>ul</c> element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Задать сообщение ошибки валидации
    /// </summary>
    public void SetMessage(string message)
    {
        modelValidationMessage = message;
        StateHasChanged();
    }

    /// <summary>
    /// Очищает сообщение ошибки валидации
    /// </summary>
    public void Clear()
    {
        modelValidationMessage = string.Empty;
        StateHasChanged();
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!string.IsNullOrWhiteSpace(modelValidationMessage))
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "alert alert-danger");
            builder.AddAttribute(2, "role", "alert");
            builder.AddMultipleAttributes(3, AdditionalAttributes);
            builder.AddContent(4, modelValidationMessage);
            builder.CloseElement();
        }
    }
}
