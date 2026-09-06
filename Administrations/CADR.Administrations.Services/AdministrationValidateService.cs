using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Services.Contracts.Exceptions;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Administrations.Services.Validators;
using CADR.Common.Core.Contracts.Models;
using CADR.Common.Core.Implementations;
using FluentValidation.Results;

namespace CADR.Administrations.Services;

/// <inheritdoc cref="IAdministrationValidateService"/>
internal sealed class AdministrationValidateService : ValidateService,
    IAdministrationValidateService,
    IAdministrationsServiceAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationValidateService"/>
    /// </summary>
    public AdministrationValidateService(IUserReadRepository userReadRepository,
        IOrganizationReadRepository organizationReadRepository)
    {
        Register<CreateUserModelValidator>(userReadRepository);
        Register<LoginModelValidator>();
        Register<CreateOrganizationModelValidator>(organizationReadRepository);
        Register<UpdateOrganizationModelValidator>(organizationReadRepository);
        Register<InviteModelValidator>();
    }

    /// <inheritdoc cref="ValidateService.ErrorsHandle"/>
    protected override Task ErrorsHandle(IEnumerable<ValidationFailure> validationFailures)
    {
        var errors = validationFailures.Select(x => InvalidateItemModel.New(x.PropertyName, x.ErrorMessage));
        throw new AdministrationValidationException(errors);
    }
}
