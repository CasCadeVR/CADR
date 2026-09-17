using CADR.Adrs.Services.Contracts.Models.Adrs;
using FluentValidation;

namespace CADR.Adrs.Services.Validators;

/// <summary>
/// Валидация <see cref="CreateAdrModel"/>
/// </summary>
public class CreateAdrModelValidator : AbstractValidator<CreateAdrModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateAdrModelValidator"/>
    /// </summary>
    public CreateAdrModelValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
    }
}
