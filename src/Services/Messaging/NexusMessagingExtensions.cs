using Microsoft.Extensions.DependencyInjection;

namespace NC.Messaging;

public static class NexusMessagingExtensions
{
    public static IServiceCollection AddNexusMessaging(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBus, RabbitMqBus>();
        return services;
    }
}