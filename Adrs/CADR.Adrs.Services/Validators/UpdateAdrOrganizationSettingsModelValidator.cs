using CADR.Adrs.Services.Contracts.Models.Settings;
using FluentValidation;

namespace CADR.Adrs.Services.Validators;

/// <summary>
/// Валидация <see cref="UpdateAdrOrganizationSettingsModel"/>
/// </summary>
public class UpdateAdrOrganizationSettingsModelValidator : AbstractValidator<UpdateAdrOrganizationSettingsModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateAdrOrganizationSettingsModelValidator"/>
    /// </summary>
    public UpdateAdrOrganizationSettingsModelValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.LikesRequiredForApproval).GreaterThanOrEqualTo(0);
    }
}
