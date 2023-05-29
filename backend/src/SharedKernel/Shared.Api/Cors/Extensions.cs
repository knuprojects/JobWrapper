using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions;

namespace Shared.Api.Cors;

public static class Extensions
{
    private static string SectionName => "cors";
    private static string PolicyName => "cors";

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.BindOptions<CorsOptions>(SectionName);
        services.AddSingleton(options);

        return services.AddCors(cors =>
        {
            var allowedHeaders = options.AllowedHeaders;
            var allowedMethods = options.AllowedMethods;    
            var allowedOrigins = options.AllowedOrigins;

            cors.AddPolicy(PolicyName, corsPolicy =>
            {
                var origins = allowedOrigins?.ToArray() ?? Array.Empty<String>();

                if (options.AllowCredentials && origins.FirstOrDefault() != "*")
                    corsPolicy.AllowCredentials();
                else
                    corsPolicy.DisallowCredentials();

                corsPolicy
                    .WithHeaders(allowedHeaders?.ToArray() ?? Array.Empty<string>())
                    .WithMethods(allowedMethods?.ToArray() ?? Array.Empty<string>())
                    .WithOrigins(origins.ToArray());
            });
        });
    }

    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
        => app.UseCors(PolicyName);
}
