using Shared;
using Users.Core;
using Users.Persistence;

namespace Users.Presentation.Infrastructure;

public static class AddDefaultServices
{
    public static IServiceCollection AddDefault(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddCore();
        services.AddPersistence();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    public static void UseDefault(this WebApplication app)
    {
        app.UseShared();

        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.Run();
    }
}
