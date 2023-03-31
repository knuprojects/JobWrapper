using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Vacancies.Core.Helpers;
using Vacancies.Core.Mapper;
using Vacancies.Core.Services;

namespace Vacancies.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IElementFinder, ElementFinder>();
        services.AddScoped<IActivateDriver, ActivateDriver>();
        services.AddScoped<IScrapperService, ScrapperService>();

        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        return services;
    }

    private static TypeAdapterConfig GetConfigureMappingConfig()
    {
        var config = new TypeAdapterConfig();

        new ResultsMapper().Register(config);

        return config;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddSingleton(GetConfigureMappingConfig());
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}
