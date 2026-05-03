using Microsoft.Extensions.DependencyInjection;
using NC.Validation.Abstractions;
using NC.Validation.Filters;
using NC.Validation.Registry;
using NC.Validation.Services;

namespace NC.Validation;

public static class ValidationDependencyInjection
{
    public static IServiceCollection AddMyValidation(this IServiceCollection services, Action<ValidatorRegistryBuilder> configure)
    {
        var builder = new ValidatorRegistryBuilder(services);
        configure(builder);

        services.AddSingleton<IValidatorRegistry>(builder.Build());
        services.AddSingleton<IValidationService, ValidationService>();

        // Register filters for DI if needed
        services.AddScoped<ValidationActionFilter>();

        return services;
    }
}