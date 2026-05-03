using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NC.AuthService.Contracts;

//using NC.AuthService.Infrastructure.Persistence.Interceptors;
using NC.AuthService.Infrastructure.Persistence.Services;

namespace NC.AuthService.Infrastructure.Persistence;

public static class AuthPersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddAuthPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("authdb")
            ?? throw new InvalidOperationException("Connection string 'authdb' is not configured.");

        //services.AddScoped<WriteOperationEventInterceptor>();
        services.AddScoped<IRoleWriteService, RoleWriteService>();

        services.AddDbContext<AuthDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString);
            //options.AddInterceptors(serviceProvider.GetRequiredService<WriteOperationEventInterceptor>());
        });

        return services;
    }
}
