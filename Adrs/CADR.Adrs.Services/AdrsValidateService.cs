using CADR.Adrs.Repositories.Contracts;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Validators;
using CADR.Common.Core.Contracts.Models;
using CADR.Common.Core.Implementations;
using FluentValidation.Results;

namespace CADR.Adrs.Services;

/// <inheritdoc cref="IAdrValidateService"/>
internal sealed class AdrsValidateService : ValidateService,
    IAdrValidateService,
    IAdrsServiceAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrsValidateService"/>
    /// </summary>
    public AdrsValidateService(IAdrFolderReadRepository adrFolderReadRepository,
        IAdrTemplateReadRepository adrTemplateReadRepository)
    {
        Register<CreateAdrModelValidator>();
        Register<UpdateAdrModelValidator>();
        Register<CreateAdrFolderModelValidator>(adrFolderReadRepository);
        Register<UpdateAdrFolderModelValidator>();
        Register<CreateAdrCommentModelValidator>();
        Register<UpdateAdrCommentModelValidator>();
        Register<CreateAdrLinkModelValidator>();
        Register<CreateAdrTemplateModelValidator>(adrTemplateReadRepository);
        Register<UpdateAdrTemplateModelValidator>();
        Register<UpdateAdrOrganizationSettingsModelValidator>();
    }

    /// <inheritdoc cref="ValidateService.ErrorsHandle"/>
    protected override Task ErrorsHandle(IEnumerable<ValidationFailure> validationFailures)
    {
        var errors = validationFailures.Select(x => InvalidateItemModel.New(x.PropertyName, x.ErrorMessage));
        throw new AdrValidationException(errors);
    }
}
