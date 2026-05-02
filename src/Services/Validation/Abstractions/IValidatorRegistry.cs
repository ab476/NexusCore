using FluentValidation;
using NC.Validation.Registry;

namespace NC.Validation.Abstractions;

public interface IValidatorRegistry
{
    bool TryGetValidator<T>(IServiceProvider serviceProvider, out IInputObjectValidator? validator);
    bool TryGetValidator(Type type, IServiceProvider serviceProvider, out IInputObjectValidator? validator);
}
