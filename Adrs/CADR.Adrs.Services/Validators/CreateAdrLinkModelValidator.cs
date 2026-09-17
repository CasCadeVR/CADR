using CADR.Adrs.Services.Contracts.Models.Links;
using CADR.Adrs.Services.Resources;
using FluentValidation;

namespace CADR.Adrs.Services.Validators;

/// <summary>
/// Валидация <see cref="CreateAdrLinkModel"/>
/// </summary>
public class CreateAdrLinkModelValidator : AbstractValidator<CreateAdrLinkModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateAdrLinkModelValidator"/>
    /// </summary>
    public CreateAdrLinkModelValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.SourceAdrId).NotEmpty();
        RuleFor(x => x.TargetAdrId).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x)
            .Must(x => x.SourceAdrId != x.TargetAdrId)
            .WithMessage(ErrorMessages.SelfLinkForbidden);
    }
}
