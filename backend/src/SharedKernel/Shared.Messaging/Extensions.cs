using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Shared.Messaging.Brokers;
using Shared.Messaging.Connections;
using Shared.Messaging.Publishers;
using Shared.Messaging.Subscribers;

namespace Shared.Messaging;

public static class Extensions
{
    public static string SectionName = "messaging";

    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        MessagingOptions messagingOptions = new();
        configuration.Bind(SectionName, messagingOptions);

        services.AddSingleton(messagingOptions);

        var factory = new ConnectionFactory
        {
            HostName = messagingOptions.HostName,
            Port = messagingOptions.Port,
            UserName = messagingOptions.UserName,
            Password = messagingOptions.Password,
            VirtualHost = messagingOptions.VirtualHost
        };

        var connection = factory.CreateConnection();

        services.AddSingleton(connection);
        services.AddSingleton<ChannelAccessor>();
        services.AddSingleton<IChannelFactory, ChannelFactory>();
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
        services.AddSingleton<IMessageSubscriber, MessageSubscriber>();
        services.AddScoped<IMessageBroker, MessageBroker>();

        return services;
    }
}
