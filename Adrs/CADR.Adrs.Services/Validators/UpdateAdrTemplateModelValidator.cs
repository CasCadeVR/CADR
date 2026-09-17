using CADR.Adrs.Services.Contracts.Models.Templates;
using FluentValidation;

namespace CADR.Adrs.Services.Validators;

/// <summary>
/// Валидация <see cref="UpdateAdrTemplateModel"/>
/// </summary>
public class UpdateAdrTemplateModelValidator : AbstractValidator<UpdateAdrTemplateModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateAdrTemplateModelValidator"/>
    /// </summary>
    public UpdateAdrTemplateModelValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
