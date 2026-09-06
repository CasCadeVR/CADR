using CADR.Administrations.Services.Contracts.Models.Invites;
using FluentValidation;

namespace CADR.Administrations.Services.Validators;

/// <summary>
/// Валидация <see cref=" InviteModel"/>
/// </summary>
public class InviteModelValidator : AbstractValidator<InviteModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="InviteModelValidator"/>
    /// </summary>
    public InviteModelValidator()
    {
        RuleFor(x => x.UserMail).NotEmpty().EmailAddress();
        RuleFor(x => x.Role).IsInEnum();
    }
}
