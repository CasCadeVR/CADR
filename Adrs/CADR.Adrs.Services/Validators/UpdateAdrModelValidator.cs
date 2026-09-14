using CADR.Adrs.Services.Contracts.Models.Adrs;
using FluentValidation;

namespace CADR.Adrs.Services.Validators;

/// <summary>
/// Валидация <see cref="UpdateAdrModel"/>
/// </summary>
public class UpdateAdrModelValidator : AbstractValidator<UpdateAdrModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateAdrModelValidator"/>
    /// </summary>
    public UpdateAdrModelValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
    }
}
