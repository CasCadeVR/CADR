using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Services.Contracts.Models.User;
using CADR.Administrations.Services.Resources;
using FluentValidation;

namespace CADR.Administrations.Services.Validators;

/// <summary>
/// Валидация <see cref="CreateUserModel"/>
/// </summary>
public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateUserModelValidator"/>
    /// </summary>
    public CreateUserModelValidator(IUserReadRepository userReadRepository)
    {
        RuleFor(x => x.Login).NotEmpty();
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(AdministrationConstants.MinimumPasswordLength);
        RuleFor(x => x.PasswordAgain)
            .NotEmpty()
            .Equal(x => x.Password);

        RuleFor(x => x.Login)
            .MustAsync(async (login, cancellationToken) =>
            {
                var exists = await userReadRepository.IsActiveLoginExistsAsync(login, cancellationToken);
                return !exists;
            })
            .WithMessage(ErrorMessages.LoginAlreadyExist);
        RuleFor(x => x.Email)
            .MustAsync(async (email, cancellationToken) =>
            {
                var exists = await userReadRepository.IsActiveEmailExistsAsync(email, cancellationToken);
                return !exists;
            })
            .WithMessage(ErrorMessages.EmailAlreadyExist);
    }
}
