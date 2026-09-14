using CADR.Adrs.Repositories.Contracts;
using CADR.Adrs.Services.Contracts.Models.Templates;
using CADR.Adrs.Services.Resources;
using FluentValidation;

namespace CADR.Adrs.Services.Validators;

/// <summary>
/// Валидация <see cref="CreateAdrTemplateModel"/>
/// </summary>
public class CreateAdrTemplateModelValidator : AbstractValidator<CreateAdrTemplateModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateAdrTemplateModelValidator"/>
    /// </summary>
    public CreateAdrTemplateModelValidator(IAdrTemplateReadRepository adrTemplateReadRepository)
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
            {
                var exists = await adrTemplateReadRepository.IsActiveNameExistsAsync(request.OrganizationId,
                    request.Name.Trim(),
                    cancellationToken);
                return !exists;
            })
            .WithMessage(ErrorMessages.TemplateAlreadyExist);
    }
}
