using CADR.Adrs.Repositories.Contracts;
using CADR.Adrs.Services.Contracts.Models.Folders;
using CADR.Adrs.Services.Resources;
using FluentValidation;

namespace CADR.Adrs.Services.Validators;

/// <summary>
/// Валидация <see cref="CreateAdrFolderModel"/>
/// </summary>
public class CreateAdrFolderModelValidator : AbstractValidator<CreateAdrFolderModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateAdrFolderModelValidator"/>
    /// </summary>
    public CreateAdrFolderModelValidator(IAdrFolderReadRepository adrFolderReadRepository)
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
            {
                var exists = await adrFolderReadRepository.IsActiveNameExistsAsync(request.OrganizationId,
                    request.ParentAdrFolderId,
                    request.Name.Trim(),
                    cancellationToken);
                return !exists;
            })
            .WithMessage(ErrorMessages.FolderAlreadyExist);
    }
}
