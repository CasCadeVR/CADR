using Microsoft.AspNetCore.Components.Forms;

namespace CADR.Portal.Components.Infrastructures;

/// <summary>
/// Css провайдер валидации формы для бутстрапа
/// </summary>
internal class BootstrapValidationClassProvider : FieldCssClassProvider
{
    /// <inheritdoc cref="FieldCssClassProvider"/>
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        if (editContext == null)
        {
            throw new ArgumentNullException(nameof(editContext));
        }

        var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

        if (editContext.IsModified(fieldIdentifier))
        {
            return isValid ? "is-valid" : "is-invalid";
        }

        return isValid ? string.Empty : "is-invalid";
    }
}
