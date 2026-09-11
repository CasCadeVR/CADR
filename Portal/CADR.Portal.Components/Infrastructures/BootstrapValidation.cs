using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace CADR.Portal.Components.Infrastructures;

/// <summary>
/// A component that configures the <see cref="EditContext"/> to use Bootstrap validation classes.
/// </summary>
public class BootstrapValidation : ComponentBase
{
    [CascadingParameter]
    private EditContext CurrentEditContext { get; set; } = default!;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException($"{nameof(BootstrapValidation)} requires a cascading parameter " +
                                                $"of type {nameof(EditContext)}. For example, you can use {nameof(BootstrapValidation)} inside " +
                                                $"an {nameof(EditForm)}.");
        }

        CurrentEditContext.SetFieldCssClassProvider(new BootstrapValidationClassProvider());
    }
}
