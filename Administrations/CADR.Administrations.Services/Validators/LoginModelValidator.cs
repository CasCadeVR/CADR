using CADR.Administrations.Services.Contracts.Models.User;
using FluentValidation;

namespace CADR.Administrations.Services.Validators;

/// <summary>
/// Валидация <see cref="LoginModel"/>
/// </summary>
public class LoginModelValidator : AbstractValidator<LoginModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LoginModelValidator"/>
    /// </summary>
    public LoginModelValidator()
    {
        RuleFor(x => x.Login).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
