using NC.Validation.Services;

namespace NC.Validation.Abstractions;

public interface IValidationService
{
    Task<FormattedValidationResponse> ValidateAsync<T>(T instance, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    Task<FormattedValidationResponse> ValidateAsync(IEnumerable<object?> instances, IServiceProvider serviceProvider, CancellationToken cancellationToken);
}