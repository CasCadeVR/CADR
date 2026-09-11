using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Services.Contracts.Models.Organizations;
using CADR.Administrations.Services.Resources;
using FluentValidation;

namespace CADR.Administrations.Services.Validators;

/// <summary>
/// Валидация <see cref="UpdateOrganizationModel"/>
/// </summary>
public class UpdateOrganizationModelValidator : AbstractValidator<UpdateOrganizationModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateOrganizationModelValidator"/>
    /// </summary>
    public UpdateOrganizationModelValidator(IOrganizationReadRepository organizationReadRepository)
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
            {
                var organization = await organizationReadRepository.GetActiveByNameAsync(request.Name.Trim().ToLower(), cancellationToken);
                var result = organization == null || organization.Id == request.Id;
                return result;
            })
            .WithName(x => nameof(x.Name))
            .WithMessage(ErrorMessages.OrganizationAlreadyExist);
    }
}
