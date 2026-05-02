using FluentValidation.Results;

namespace NC.Validation.Registry;

public interface IInputObjectValidator
{
    Task<ValidationResult?> ValidateAsync(object instance, CancellationToken cancellationToken);
}