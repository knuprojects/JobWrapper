using Microsoft.Extensions.DependencyInjection;
using Shared.Dal;

namespace Vacancies.Persistence
{
    public static class Extensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
            => services.AddPostgresDatabase<VacancyContext>();
    }
}
