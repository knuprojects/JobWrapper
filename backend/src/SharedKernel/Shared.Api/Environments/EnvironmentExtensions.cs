using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Shared.Api.Environments;

public static class EnvironmentExtensions
{
    public static IConfiguration SetEnvironmentConfiguration(this IConfiguration configuration, IWebHostEnvironment environment)
    {
        return new ConfigurationBuilder()
            .SetBasePath(configuration.GetSection("contentRoot").Value)
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables().Build();
    }

    public static string GetCurrentEnvironment(IWebHostEnvironment environment)
        => $"Environment: {environment.EnvironmentName}";
}