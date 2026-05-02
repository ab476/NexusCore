using FluentValidation;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NC.Validation.Abstractions;

namespace NC.Validation.Registry;

public delegate IInputObjectValidator InputObjectValidatorFactory(IServiceProvider serviceProvider);
public class ValidatorRegistryBuilder(IServiceCollection services)
{
    private readonly ConcurrentDictionary<Type, InputObjectValidatorFactory> _validators = new();
    private readonly IServiceCollection _services = services;

    public ValidatorRegistryBuilder AddValidator<T, TValidator>()
        where T : class
        where TValidator : class, IValidator<T>
    {
        _services.TryAddSingleton<IValidator<T>, TValidator>();
        _validators[typeof(T)] = static serviceProvider =>
            new InputObjectValidator<T>(serviceProvider.GetRequiredService<IValidator<T>>());
        return this;
    }
    public IValidatorRegistry Build() => new ValidatorRegistry(_validators);
}