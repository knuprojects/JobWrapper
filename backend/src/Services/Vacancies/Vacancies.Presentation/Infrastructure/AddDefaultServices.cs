using Shared;
using Vacancies.Core;
using Vacancies.Persistence;

namespace Vacancies.Presentation.Infrastructure;

public static class AddDefaultServices
{
    public static IServiceCollection AddDefault(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddCore();
        services.AddMapper();
        services.AddPersistence();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static void UseDefault(this WebApplication app)
    {
        app.UseShared();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        app.Run();
    }
}
