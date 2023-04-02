using Shared;
using Shared.Abstractions;
using Vacancies.Core;
using Vacancies.Core.Common.Options;
using Vacancies.Persistence;

namespace Vacancies.Presentation.Infrastructure;

public static class AddDefaultServices
{
    private static string DjinniSectionName => "djinni";

    public static IServiceCollection AddDefault(this IServiceCollection services, IConfiguration configuration)
    {
        var djinniOptions = configuration.BindOptions<DjinniOptions>(DjinniSectionName);
        services.AddSingleton(djinniOptions);

        services.AddControllers();
        services.AddCore();
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
