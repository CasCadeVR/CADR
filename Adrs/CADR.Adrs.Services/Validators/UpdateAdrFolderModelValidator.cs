using CADR.Adrs.Services.Contracts.Models.Folders;
using FluentValidation;

namespace CADR.Adrs.Services.Validators;

/// <summary>
/// Валидация <see cref="UpdateAdrFolderModel"/>
/// </summary>
public class UpdateAdrFolderModelValidator : AbstractValidator<UpdateAdrFolderModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateAdrFolderModelValidator"/>
    /// </summary>
    public UpdateAdrFolderModelValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
