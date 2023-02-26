using Microsoft.Extensions.DependencyInjection;
using Shared.Messaging.Brokers;
using Shared.Messaging.Publishers;

namespace Shared.Messaging;

public static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddScoped<IMessageBroker, MessageBroker>();
        services.AddScoped<IMessagePublisher, MessagePublisher>();

        return services;
    }
}
