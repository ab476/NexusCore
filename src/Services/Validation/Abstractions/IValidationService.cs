using NC.Validation.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NC.Validation.Abstractions;

public interface IValidationService
{
    Task<FormattedValidationResponse> ValidateAsync<T>(T instance, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    Task<FormattedValidationResponse> ValidateAsync(IEnumerable<object?> instances, IServiceProvider serviceProvider, CancellationToken cancellationToken);
}