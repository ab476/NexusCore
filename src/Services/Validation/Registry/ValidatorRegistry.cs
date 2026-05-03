using NC.Validation.Abstractions;
using System.Collections.Concurrent;

namespace NC.Validation.Registry;

internal class ValidatorRegistry(ConcurrentDictionary<Type, InputObjectValidatorFactory> validators) : IValidatorRegistry
{
    private readonly ConcurrentDictionary<Type, InputObjectValidatorFactory> _validators = validators;

    public bool TryGetValidator<T>(IServiceProvider serviceProvider, out IInputObjectValidator? validator)
        => TryGetValidator(typeof(T), serviceProvider, out validator);
    public bool TryGetValidator(Type type, IServiceProvider serviceProvider, out IInputObjectValidator? validator)
    {

        if (_validators.TryGetValue(type, out var validatorFac))
        {
            validator = validatorFac(serviceProvider);
            return true;
        }

        validator = default;
        return false;
    }
}
