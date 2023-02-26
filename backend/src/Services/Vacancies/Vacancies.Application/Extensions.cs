using Microsoft.Extensions.DependencyInjection;
using Vacancies.Application.Drivers.Services;

namespace Vacancies.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
             => services
              .AddScoped<IActivateDriver, ActivateDriver>();
}
