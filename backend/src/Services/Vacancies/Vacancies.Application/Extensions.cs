using Microsoft.Extensions.DependencyInjection;
using Vacancies.Application.Drivers.Services;
using Vacancies.Application.Vacancies.Djinni.Interfaces;
using Vacancies.Application.Vacancies.Djinni.Services;

namespace Vacancies.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
             => services
              .AddScoped<IActivateDriver, ActivateDriver>()
              .AddScoped<IScrapingVacanciesDjinni, ScrapingVacanciesDjinni>();
}
