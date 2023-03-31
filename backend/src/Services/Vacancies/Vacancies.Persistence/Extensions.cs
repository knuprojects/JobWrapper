using Microsoft.Extensions.DependencyInjection;
using Shared.Dal;
using Vacancies.Core.Repositories;
using Vacancies.Persistence.Repositories;

namespace Vacancies.Persistence
{
    public static class Extensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
      => services
               .AddScoped<IVacancyRepository, VacancyRepository>().AddPostgresDatabase<VacancyContext>();
    }
}
