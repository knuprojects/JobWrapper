using Microsoft.Extensions.DependencyInjection;
using Vacancies.Core.Helpers;
using Vacancies.Core.Services;

namespace Vacancies.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
        => services
                //.AddSingleton<IWebDriver>()
                .AddSingleton<IElementFinder, ElementFinder>()
                .AddScoped<IActivateDriver, ActivateDriver>()
                .AddScoped<IScrapperService, ScrapperService>();
}
