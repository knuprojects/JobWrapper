using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.AdditionalAbstractions;
using Shared.Abstractions.Serialization;
using Shared.Abstractions.Time;
using System.Reflection;

namespace Shared.Abstractions;

public static class Extensions
{
    public static T BindOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        => BindOptions<T>(configuration.GetSection(sectionName));

    public static T BindOptions<T>(this IConfigurationSection section) where T : new()
    {
        var options = new T();
        section.Bind(options);
        return options;
    }

    public static IServiceCollection AddAbstractions(this IServiceCollection services)
        => services
                   .AddScoped<IUtcClock, UtcClock>()
                   .AddSingleton<IJsonSerializer, TextJsonSerializer>();

    public static IServiceCollection AddDispatchers(this IServiceCollection services, Assembly assemblies)
    {
        services.AddSingleton<IEventDispatcher, EventDispatcher>();

        services.Scan(cfg => cfg.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        return services;
    }
}