using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Services.Contracts.Models.Organizations;
using CADR.Administrations.Services.Resources;
using FluentValidation;

namespace CADR.Administrations.Services.Validators;

/// <summary>
/// Валидация <see cref="CreateOrganizationModel"/>
/// </summary>
public class CreateOrganizationModelValidator : AbstractValidator<CreateOrganizationModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateOrganizationModelValidator"/>
    /// </summary>
    public CreateOrganizationModelValidator(IOrganizationReadRepository organizationReadRepository)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name)
            .MustAsync(async (name, cancellationToken) =>
            {
                var exists = await organizationReadRepository.IsActiveNameExistsAsync(name, cancellationToken);
                return !exists;
            })
            .WithMessage(ErrorMessages.OrganizationAlreadyExist);
    }
}
