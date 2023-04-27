using Microsoft.Extensions.DependencyInjection;
using Users.Core.Helpers;

namespace Users.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<ITokenStorage, HttpTokenStorage>();

        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        return services;
    }
}