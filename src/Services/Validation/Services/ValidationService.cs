using FluentValidation.Results;
using NC.Validation.Abstractions;

namespace NC.Validation.Services;

public sealed class ValidationService : IValidationService
{
    private readonly IValidatorRegistry _registry;

    public ValidationService(IValidatorRegistry registry)
    {
        _registry = registry;
    }

    public async Task<FormattedValidationResponse> ValidateAsync<T>(
        T instance,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        if (instance is null)
            return new FormattedValidationResponse();

        if (!_registry.TryGetValidator<T>(serviceProvider, out var validator))
            return new FormattedValidationResponse();

        var result = await validator.ValidateAsync(instance, cancellationToken);

        return new FormattedValidationResponse
        {
            Errors = MapErrors(result)
        };
    }

    public async Task<FormattedValidationResponse> ValidateAsync(
        IEnumerable<object?> instances,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var errors = new List<ValidationError>();

        foreach (var instance in instances)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (instance is null)
                continue;

            if (!_registry.TryGetValidator(instance.GetType(), serviceProvider, out var validator))
                continue;

            var result = await validator.ValidateAsync(instance, cancellationToken);

            if (!result.IsValid)
                errors.AddRange(MapErrors(result));
        }

        return new FormattedValidationResponse
        {
            Errors = errors
        };
    }

    private static List<ValidationError> MapErrors(ValidationResult result)
        => result.Errors
            .Select(e => new ValidationError
            {
                Property = e.PropertyName,
                Message = e.ErrorMessage
            })
            .ToList();
}