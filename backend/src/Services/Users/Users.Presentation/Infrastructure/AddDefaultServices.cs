using MassTransit;
using Shared;
using Shared.Abstractions;
using Shared.Messaging;
using Users.Core;
using Users.Persistence;

namespace Users.Presentation.Infrastructure;

public static class AddDefaultServices
{
    private static string SectionName => "messaging";

    public static IServiceCollection AddDefault(this IServiceCollection services, IConfiguration configuration)
    {
        var messagingOptions = configuration.BindOptions<MessagingOptions>(SectionName);
        services.AddSingleton(messagingOptions);

        services.AddCore();
        services.AddPersistence();
        services.AddControllers();

        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.UsingRabbitMq((context, rabbitConfig) =>
            {
                rabbitConfig.Host(messagingOptions.HostName);
            });
        });

        services.AddMessaging();

        return services;
    }

    public static void UseDefault(this WebApplication app)
    {
        app.UseShared();

        app.MapControllers();

        app.Run();
    }
}
