using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Shared.Abstractions;

namespace Shared.Http;

public static class Extensions
{
    private static string SectionName = "httpClient";

    public static IHttpClientBuilder AddCustomHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var httpClientOptions = configuration.BindOptions<HttpClientOptions>(SectionName);
        services.AddSingleton(httpClientOptions);

        var builder = services
        .AddHttpClient(httpClientOptions.Name)
        .AddTransientHttpErrorPolicy(_ => HttpPolicyExtensions.HandleTransientHttpError()
            .WaitAndRetryAsync(httpClientOptions.Resiliency.Retries, retry =>
                httpClientOptions.Resiliency.Exponential
                    ? TimeSpan.FromSeconds(Math.Pow(2, retry))
                    : httpClientOptions.Resiliency.RetryInterval ?? TimeSpan.FromSeconds(2)));

        return builder;
    }
}
