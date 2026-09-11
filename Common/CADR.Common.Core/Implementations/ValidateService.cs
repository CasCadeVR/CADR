using CADR.Common.Core.Contracts;
using FluentValidation;
using FluentValidation.Results;

namespace CADR.Common.Core.Implementations;

/// <summary>
/// Сервис валидации
/// </summary>
public abstract class ValidateService : IValidateService
{
    private readonly IDictionary<Type, IValidator> validators;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ValidateService"/>
    /// </summary>
    protected ValidateService()
    {
        validators = new Dictionary<Type, IValidator>();
    }

    /// <summary>
    /// Регистрирует валидатор в словаре
    /// </summary>
    public void Register<TValidator>(params object[] constructorParams)
        where TValidator : IValidator
    {
        var validatorType = typeof(TValidator);
        var innerType = validatorType.BaseType?.GetGenericArguments()[0];
        if (innerType == null)
        {
            throw new ArgumentNullException($"Указанный валидатор {validatorType} должен быть generic от типа IValidator");
        }

        if (constructorParams?.Any() == true)
        {
            var validatorObject = Activator.CreateInstance(validatorType, constructorParams);
            if (validatorObject is IValidator validator)
            {
                validators.TryAdd(innerType, validator);
            }
        }
        else
        {
            validators.TryAdd(innerType, Activator.CreateInstance<TValidator>());
        }
    }

    /// <inheritdoc cref="IValidateService.ValidateAsync{TModel}"/>
    public async Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken)
        where TModel : class
    {
        validators.TryGetValue(model.GetType(), out var validator);

        if (validator != null)
        {
            var context = new ValidationContext<TModel>(model);
            var validationResult = await validator.ValidateAsync(context, cancellationToken);
            if (!validationResult.IsValid)
            {
                await ErrorsHandle(validationResult.Errors);
            }
        }
    }

    /// <summary>
    /// Обработка ошибок валидации
    /// </summary>
    protected abstract Task ErrorsHandle(IEnumerable<ValidationFailure> validationFailures);
}
