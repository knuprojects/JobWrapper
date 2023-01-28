using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Serialization;
using Shared.Abstractions.Time;

namespace Shared.Abstractions;

public static class Extensions
{
    public static IServiceCollection AddAbstractions(this IServiceCollection services)
        => services
                   .AddScoped<IUtcClock, UtcClock>()
                   .AddSingleton<IJsonSerializer, TextJsonSerializer>();
}