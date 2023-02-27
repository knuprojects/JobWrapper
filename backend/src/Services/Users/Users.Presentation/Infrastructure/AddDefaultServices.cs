using Shared;
using Users.Persistence;

namespace Users.Presentation.Infrastructure;

public static class AddDefaultServices
{
    public static IServiceCollection AddDefault(this IServiceCollection services)
    {
        services.AddPersistence();
        services.AddControllers();

        return services;
    }

    public static void UseDefault(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseShared();

        app.MapControllers();

        app.Run();
    }
}
